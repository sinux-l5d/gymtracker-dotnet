#!/bin/bash -e

# usage: destroy.sh <stack-name|stack-arn>
# default value for stack-name is get from the file stack_arn.txt

# usage function
usage() {
    echo "usage: destroy.sh <stack-name|stack-arn>"
    echo "  defaults: stack-name=\`cat stack_arn.txt\`"
}

# check for correct number of arguments
if [ $# -gt 1 ]; then
    usage
    exit 1
fi

if [ "$1" == "--help" ] || [ "$1" == "-h" ]; then
    usage
    exit 0
fi

STACK_NAME=${1:-$(cat stack_arn.txt)}

# if empty, abort
if [ -z "$STACK_NAME" ]; then
    echo "No stack name or ARN provided (or found in stack_arn.txt)"
    usage
    exit 1
fi

spinner()
{
    local pid=$1
    local delay=0.75
    local spinstr='|/-\'
    while [ "$(ps a | awk '{print $1}' | grep $pid)" ]; do
        local temp=${spinstr#?}
        printf " [%c]  " "$spinstr"
        local spinstr=$temp${spinstr%"$temp"}
        sleep $delay
        printf "\b\b\b\b\b\b"
    done
    printf "    \b\b\b\b"
}

# check if stack exists
STACK_EXISTS=$(aws cloudformation describe-stacks --stack-name $STACK_NAME 2>/dev/null)
if [ $? -ne 0 ]; then
    echo "Stack $STACK_NAME does not exist"
    exit 1
fi

# check if stack is in a valid state
STACK_STATUS=$(echo $STACK_EXISTS | jq -r '.Stacks[0].StackStatus')
if [ "$STACK_STATUS" != "CREATE_COMPLETE" ] && [ "$STACK_STATUS" != "UPDATE_COMPLETE" ]; then
    echo "Stack $STACK_NAME is in state $STACK_STATUS, cannot delete"
    exit 1
fi

# delete stack
echo "Deleting stack $STACK_NAME"
aws cloudformation delete-stack --stack-name "$STACK_NAME"

# wait for stack to be deleted
echo -n "Waiting for stack $STACK_NAME to be deleted"
aws cloudformation wait stack-delete-complete --stack-name "$STACK_NAME" & spinner $!
echo

rm stack_arn.txt
echo "Stack $STACK_NAME deleted (and stack_arn.txt removed)"

echo