#!/bin/bash -e

# usage: launch.sh <base-name> <aws-keypair-name>
# default value for base-name is "SOA-CA2"
# default value for aws-keypair-name is "MAIN_KEY"

# usage function
usage() {
    echo "usage: launch.sh <base-name> <aws-keypair-name>"
    echo "  defaults: base-name=SOA-CA2, aws-keypair-name=MAIN_KEY"
}

# check for correct number of arguments
if [ $# -gt 2 ]; then
    usage
    exit 1
fi

if [ "$1" == "--help" ] || [ "$1" == "-h" ]; then
    usage
    exit 0
fi

BASE_NAME=${1:-SOA-CA2}
AWS_KEYPAIR_NAME=${2:-MAIN_KEY}

STACK_NAME="${BASE_NAME}-Stack"

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

echo "Creating stack $STACK_NAME"
STACK_ARN=$(aws cloudformation create-stack \
  --stack-name "$STACK_NAME" \
  --template-body file://soa-vpc.yml \
  --parameters ParameterKey=BaseName,ParameterValue="${BASE_NAME//-/_}" \
               ParameterKey=KeyName,ParameterValue="$AWS_KEYPAIR_NAME" \
  | jq -r '.StackId')

# trim and store
echo "$STACK_ARN" | xargs >> "./stack_arn.txt"

echo -n "Waiting for stack to be created "
aws cloudformation wait stack-create-complete --stack-name $STACK_ARN & spinner $!
echo

VPC_IP=$(aws cloudformation describe-stacks --stack-name $STACK_ARN --query "Stacks[0].Outputs[?OutputKey=='PublicIp'].OutputValue" --output text)

echo "VPC IP: $VPC_IP"
echo "You can connect to the VPC with: ssh -i ~/.ssh/YOU_PRIVATE_KEY.pem ec2-user@$VPC_IP"
