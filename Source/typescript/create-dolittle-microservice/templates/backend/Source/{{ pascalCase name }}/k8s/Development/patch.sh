#!/bin/bash
kubectl patch --namespace application-{{applicationId}} deployment {{lowerCase applicationName}}-dev-{{lowerCase name}} -p '{"spec": { "template": {"metadata": { "labels": { "date": "'`date +'%s'`'" } }}}}'
