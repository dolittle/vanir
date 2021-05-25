# CLI - Features

## Context

The `features` command group only works while inside a folder that
holds features configuration, typically within the **backend** part of your microservice.

## List features

```shell
$ vanir features list

FEATURES FOR
Application : SampleApp (77628df0-f46f-2e46-87a6-bc5a75578bcf)
Microservice : AspNetCore (6e24ebf1-f724-a54d-9553-1c61f5e9f83f)

NAME                     DESCRIPTION                        IS ON
my.first.feature         The ultimate first feature         False
my.second.feature        The ultimate second feature        True
```

## Add feature

```shell
$ vanir features add my.third.feature 'The ultimate third feature'

FEATURES FOR
Application : SampleApp (77628df0-f46f-2e46-87a6-bc5a75578bcf)
Microservice : AspNetCore (6e24ebf1-f724-a54d-9553-1c61f5e9f83f)

NAME               DESCRIPTION                  IS ON
my.first.feature   The ultimate first feature   False
my.second.feature  The ultimate second feature  True
my.third.feature   The ultimate third feature   False
```

## Remove feature

```shell
$ vanir features remove my.third.feature

FEATURES FOR
Application : SampleApp (77628df0-f46f-2e46-87a6-bc5a75578bcf)
Microservice : AspNetCore (6e24ebf1-f724-a54d-9553-1c61f5e9f83f)

NAME               DESCRIPTION                  IS ON
my.first.feature   The ultimate first feature   False
my.second.feature  The ultimate second feature  True
```

## Switch on feature

```shell
$ vanir features switch-on my.first.feature

FEATURES FOR
Application : SampleApp (77628df0-f46f-2e46-87a6-bc5a75578bcf)
Microservice : AspNetCore (6e24ebf1-f724-a54d-9553-1c61f5e9f83f)

NAME               DESCRIPTION                  IS ON
my.first.feature   The ultimate first feature   True
my.second.feature  The ultimate second feature  True
```

## Switch off feature

```shell
$ vanir features switch-off my.second.feature

FEATURES FOR
Application : SampleApp (77628df0-f46f-2e46-87a6-bc5a75578bcf)
Microservice : AspNetCore (6e24ebf1-f724-a54d-9553-1c61f5e9f83f)

NAME               DESCRIPTION                  IS ON
my.first.feature   The ultimate first feature   True
my.second.feature  The ultimate second feature  False
```
