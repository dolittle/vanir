#!/bin/bash
kubectl patch --namespace application-{{applicationId}} deployment {{lowerCase applicationName}}-dev-{{lowercase name}} -p '{"spec": { "template": {"metadata": { "labels": { "date": "'`date +'%s'`'" } }}}}'
