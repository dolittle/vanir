#!/bin/bash
kubectl patch --namespace application-{{applicationId}} deployment {{lowerCase applicationName}}-prod-{{lowerCase name}}
