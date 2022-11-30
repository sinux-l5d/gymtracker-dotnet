# Gym tracker

This API was created for the SOA class in [Dkit](https://www.dkit.ie/).

## How to run

You need dotnet 6.0 installed on your machine to run by hand.

With docker:

```bash
docker run -d -p 80:80 -p 443:443 --name gymtracker ghcr.io/sinux-l5d/gymtracker-dotnet:latest
```

## Deploy to AWS

You need to have the AWS CLI installed and configured.

Example:

```bash
cd aws
./launch.sh SOA-CA2 MY_KEY_NAME_IN_AWS
```

To destroy the stack:

```bash
cd aws
./destroy.sh
```

this will read the stack_arn.txt file if no stack arn/name is provided.