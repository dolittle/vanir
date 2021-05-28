#!/bin/bash
kubectl patch --namespace application-77628df0-f46f-2e46-87a6-bc5a75578bcf deployment sampleapp-dev-typescript -p '{"spec": { "template": {"metadata": { "labels": { "date": "'`date +'%s'`'" } }}}}'
