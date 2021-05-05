# [9.22.1] - 2021-5-5 [PR: #212](https://github.com/dolittle/vanir/pull/212)
### Added

- Noddin


# [9.22.0] - 2021-5-5 [PR: #208](https://github.com/dolittle/vanir/pull/208)
### Added

- [C#] Added a default public filter that will forward all events applied/committed as public - this behavior can be overridden by setting `PublishAllPublicEvents` in the `BackendArguments` used in `AddVanir()` method to false. You can then use the Dolittle configuration callback to configure your filter instead: https://dolittle.io/docs/tutorials/event-horizon/
- [C#] All types adorned with the [Projection] attribute are automatically registered as projections. Read more here: https://dolittle.io/docs/tutorials/projections/

### Changed

- [C#] Upgraded to version 8.4.0 of the SDK

# [9.21.1] - 2021-4-29 [PR: #204](https://github.com/dolittle/vanir/pull/204)
### Fixed

- [C#] During GraphQL calls, all validators for a type using FluentValidation is now included automatically. Not just the first it discovers.


# [9.21.0] - 2021-4-28 [PR: #201](https://github.com/dolittle/vanir/pull/201)
### Added

- [C#] Added a callback to the BackendArguments to be able to configure the Mongo DB settings before they're frozen for the client instance


# [9.20.2] - 2021-4-27 [PR: #199](https://github.com/dolittle/vanir/pull/199)
### Fixed

- Apollo + GraphQL dependencies moved from React to Web - which is more correct. If you don't use React, you'll then get the dependencies we really rely on.


# [9.20.1] - 2021-4-25 [PR: #198](https://github.com/dolittle/vanir/pull/198)
### Changed

- @dolittle/projections upgraded to v2.1.0 for supporting Guid conversion to and from Mongo


# [9.20.0] - 2021-4-21 [PR: #191](https://github.com/dolittle-entropy/vanir/pull/191)
### Added

- Support for @graphRoot decorator to be able to root resolver classes and resolver methods to build a structured schema.
- Preliminary support for calling event mutations directly - without type details - you give it a JSON string as event content / payload


# [9.19.1] - 2021-4-21 [PR: #190](https://github.com/dolittle-entropy/vanir/pull/190)
### Fixed

- Fixed so that Messenger doesn't publish a message that came from the 'portal' back to the 'portal' from a content frame
- Added the missing ? search/query string key/value to the params
- Make sure to pass the search/query string along to the ContentFrame from the portal frame


# [9.19.0] - 2021-4-14 [PR: #182](https://github.com/dolittle-entropy/vanir/pull/182)
### Added

- eventTypes and eventHandlerTypes can now be configured during start of host
- BrokenRule, typically used in aggregates to convey when something is not allowed - a broken rule
- Aggregate support for IoC purposes

### Changed

- Upgraded to version 14.3.0 of the TypeScript SDK
- Upgraded to version 2.0.0 of the Dolittle declarative projections
- Upgraded samples to use vrsion 5.5.0 of the Dolittle Runtime


# [9.18.3] - 2021-4-11 [PR: #174](https://github.com/dolittle-entropy/vanir/pull/174)
### Added

- Documentation for working with the frontend and specifically React and related to the MVVM model. Fixes #172, #166, #165

### Changed

- Patch level change - non breaking - callbacks for ContentFrame / CompositionRoute is now optional

### Fixed

- Making sure to cause re-render when route changes occur as a consequence of using the `INavigator`. Fixes #170.


# [9.18.2] - 2021-4-5 [PR: #163](https://github.com/dolittle-entropy/vanir/pull/163)
### Fixed

- Enabling queries and mutations to live on the same route by postfixing the types used within Query and Mutation types and their children. Postifxes are "Queries" and "Mutations" for the types.


# [9.18.1] - 2021-4-5 [PR: #162](https://github.com/dolittle-entropy/vanir/pull/162)
### Fixed

- Event mutations should only be exposed when running in development

# [9.18.0] - 2021-4-5 [PR: #162](https://github.com/dolittle-entropy/vanir/pull/161)
### Added

- Exposing all event types based on types discovered into the GraphQL schema under mutation. Names are linked to the typename. This allowes for `mutation { events { ...nameOfEventType } }`

# [9.17.0] - 2021-4-4 [PR: #160](https://github.com/dolittle-entropy/vanir/pull/160)
### Added

- Aggregate root support through dependency injection of IAggregateOf<>
- Updated dependencies to the latest version of the Dolittle SDK (8.3.2)


# [9.16.0] - 2021-4-4 [PR: #159](https://github.com/dolittle-entropy/vanir/pull/159)
### Added

- `ITypes` now exposes the assemblies used for type discovery
- Reflection utility methods for working with attributes and interfaces
- Dolittle Runtime support implemented with automatic discovery of EventTypes and EventHandlers - it also hooks up `IEventStore` propely as a dependency that can be used and is resolved in the right context.


# [9.15.0] - 2021-4-4 [PR: #158](https://github.com/dolittle-entropy/vanir/pull/158)
### Added

- `AddVanir()` now returns a `Services` object that holds for now the `ITypes` for type discovery but can then be expanded on in the future.


# [9.14.0] - 2021-4-3 [PR: #157](https://github.com/dolittle-entropy/vanir/pull/157)
### Fixed

- Removes ".Value" for concepts being set using .SetValidator typically
- Does not add errors that are already there - avoids duplicate error messages for same proprty if one has added multiple instances of same validator


# [9.13.2] - 2021-4-3 [PR: #156](https://github.com/dolittle-entropy/vanir/pull/156)
### Fixed

- Skip properties in validation recursion when property type is of same as parent to avoid stack overflow exception


# [9.13.1] - 2021-4-3 [PR: #155](https://github.com/dolittle-entropy/vanir/pull/155)
### Fixed

- Avoid getting duplicate keys for the population of validation types by type


# [9.13.0] - 2021-4-3 [PR: #154](https://github.com/dolittle-entropy/vanir/pull/154)
### Added

- `GraphRoot` attribute that will enable you to set the root object - can be nested, delimited by /
- `Query` attribute now supports path relative to `GraphRoot` or root if not `GraphRoot` is used - can be nested, delimited by /
- `Mutation` attribute now supports path relative to `GraphRoot` or root if not `GraphRoot` is used - can be nested, delimited by /
- Support for FluentValidations AbstractValidator - automatically hooked up on any input arguments
- AbstractValidator<> can be for any type - any input type with a property of a type with a validator will be evaluated as well


# [9.10.0] - 2021-3-31 [PR: #152](https://github.com/dolittle-entropy/vanir/pull/152)
### Added

- Enum naming convention adds a suffix of enum to type name


# [9.9.0] - 2021-3-31 [PR: #151](https://github.com/dolittle-entropy/vanir/pull/151)
### Added

- ConceptAs extensions to make it easier to work with ConceptAs
- Reflection extensions to make it easier to work with type reflection
- Automatic hookup of Type conversion for GraphQL for ConceptAs and base type mapping. This enables use of concepts in any GraphQL construct (ObjectType) and it will map to the underlying system type it is a concept as of.


# [9.8.0] - 2021-3-30 [PR: #150](https://github.com/dolittle-entropy/vanir/pull/150)
### Added

- Convenient Collection Extensions
- Simple type discovery for project referenced assemblies
- Automatic hook up of any `ScalarType` / `ScalarType<>` implementations
- Ability to configure Hot Chocolate GraphQL properties manually


# [9.7.0] - 2021-3-30 [PR: #149](https://github.com/dolittle-entropy/vanir/pull/149)
### Fixed

- When paths are rooted as described in `vanir.json` - the GraphQL paths will now be single / at the beginning.


# [9.6.0] - 2021-3-30 [PR: #148](https://github.com/dolittle-entropy/vanir/pull/148)
## Summary

Making it simpler to work with Hot Chocolate type of GraphQL - heavily influenced by GraphQL AspNetCore.


### Added

- GraphController, QueryAttribute nad MutationAttribute - as found in GraphQL AspNetCore


# [9.5.0] - 2021-3-20 [PR: #147](https://github.com/dolittle-entropy/vanir/pull/147)
## Summary

Shaping up the C# setup for resources, graphql and all.

### Added

- [C#] Support for taking dependency to IMongoDatabase and it resolve to correct tenant based on resources.json config file
- [C#] GraphQL routes adhering to the vanir.json file setup
- [C#] ExecutionContext setup properly based on the Tenant-ID HTTP header
- [C#] Microservice.json configuration support added - ExecutionContext leverages this


# [9.4.0] - 2021-3-20 [PR: #146](https://github.com/dolittle-entropy/vanir/pull/146)
## Summary

Shaping up the C# setup for resources, graphql and all.

### Added

- [C#] Support for taking dependency to IMongoDatabase and it resolve to correct tenant based on resources.json config file
- [C#] GraphQL routes adhering to the vanir.json file setup
- [C#] ExecutionContext setup properly based on the Tenant-ID HTTP header
- [C#] Microservice.json configuration support added - ExecutionContext leverages this


# [9.1.3] - 2021-3-17 [PR: #143](https://github.com/dolittle-entropy/vanir/pull/143)
## Summary

This fixes an undesired breaking change from version 8 to 9. Collection names would by default be the type name- but we expect the same default behavior as with Mongoose, which is making it lowercase and pluralized. This particular code is now brought in from Mongoose to make sure we have the same behavior and is as expected.

### Fixed

- Collection names are now correct; lowercase + pluralized


# [9.1.2] - 2021-3-15 [PR: #142](https://github.com/dolittle-entropy/vanir/pull/142)
## Summary

Making GuidScalar more available

### Fixed

- Exporting GuidScalar from root module @dolittle/vanir-backend


# [9.1.1] - 2021-3-15 [PR: #140](https://github.com/dolittle-entropy/vanir/pull/140)
## Summary

Exporting sub modules.

### Fixed

- To avoid consumers of packages having to import `dist` relative paths within the package, we now export the sub-modules as part of the top-level package (e.g. data will be @dolittle/vanir-backend/data instead of @dolittle/vanir-backend/dist/data)


# [9.1.0] - 2021-3-14 [PR: #138](https://github.com/dolittle-entropy/vanir/pull/138)
## Summary

Improved developer experience and flexibility for working with resources such as mongo database and the Dolittle even store.

### Added

- Documentation on available dependencies
- MongoDatabaseProvider for getting IMongoDatabase instances for specific tenant (Fixes [#128](https://github.com/dolittle-entropy/vanir/issues/128))
- Adds the Dolittle client as an available dependency (Fixes [#127](https://github.com/dolittle-entropy/vanir/issues/127)).

### Fixed

- Exports at root level of backend - exposing common types to avoid having '/dist' in imports


# [9.0.9] - 2021-3-14 [PR: #136](https://github.com/dolittle-entropy/vanir/pull/136)
## Summary

Fixing the scaffolding.

### Fixed

- Fixes [#135](https://github.com/dolittle-entropy/vanir/issues/135) - casing issue in templates.


# [9.0.8] - 2021-3-10 [PR: #133](https://github.com/dolittle-entropy/vanir/pull/133)
## Summary

Makes scaffolding of new projects correct and also makes sure `IEventStore` instance is for the correct tenant.

### Fixed

- Fixes so we don't use webpack-cli but webpack directly for yarn:dev scripts in package.json
- When taking `IEventStore` as a dependency - it is now resolved using the correct tenant-id ([#125](https://github.com/dolittle-entropy/vanir/issues/125))


# [9.0.7] - 2021-3-3 [PR: #132](https://github.com/dolittle-entropy/vanir/pull/132)
## Summary

update prod configmap template to reflect new structure

### Fixed

- update prod configmap template to reflect new structure


# [9.0.3] - 2021-3-2 [PR: #126](https://github.com/dolittle-entropy/vanir/pull/126)
## Summary

Making sure we have the right context both for regular Express middlewares and GraphQL.

### Fixed

- Fixes so that the context is correct in all call paths


# [9.0.2] - 2021-2-28 [PR: #123](https://github.com/dolittle-entropy/vanir/pull/123)
## Summary

Fixes so that the correct context is returned.

### Fixed

- Fixes so that the correct context is returned - not the default one.


# [9.0.1] - 2021-2-28 [PR: #122](https://github.com/dolittle-entropy/vanir/pull/122)
## Summary

Typo when getting HTTP header for Tenant

### Fixed

- Fixing usage of correct HTTP header for tenant; Tenant-ID not Tenant-Id


# [9.0.0] - 2021-2-26 [PR: #118](https://github.com/dolittle-entropy/vanir/pull/118)
## Summary

This pull request is a major change.
The biggest change is the removal of TypeGoose and Mongoose as default - instead it leverages now the native MongoDB driver and all setup is configured for this.

### Added

- Resource system for getting resource configurations per type per tenant (#111)
- Introducing an unobtrusive way to work with MongoDB driver and still get the benefits of custom serialization with custom types.
- Context object configured through middleware containing the tenant identifier, user id and cookies. (#112)

### Changed

- Changing from TypeGoose to native MongoDB driver for NodeJS and configured it by default
- TenantId on Context is now the type TenantId from @dolittle/sdk.execution

### Removed

- TypeGoose and Mongoose are both moved as dependency and hence not configured by default as a consequence
- Environment variables for configuring database and event store access


# [8.0.5] - 2021-2-18 [PR: #117](https://github.com/dolittle-entropy/vanir/pull/117)
## Summary

This PR fixes the hardcoded "Dolittle" tenant in the k8s production configuration to be configurable per template

### Fixed

- Fix the hardcoded "Dolittle" tenant in the k8s production configuration to be configurable per template


# [8.0.4] - 2021-2-17 [PR: #115](https://github.com/dolittle-entropy/vanir/pull/115)
## Summary

We've had issues with WebPack when using the dev-server and also all of a sudden broken builds due to the html-webpack-plugin not loading the ejs template file and rendering it.

### Fixed

- Upgraded to latest WebPack
- Upgraded to latest html-webpack-plugin
- Removing --watch option
- Updating templates to use webpack instead of webpack-cli directly


# [8.0.3] - 2021-2-16 [PR: #114](https://github.com/dolittle-entropy/vanir/pull/114)
## Summary

Locking down remaining dependencies on specific version.


### Fixed

- Further lockdown on version for 3rd parties - must have missed some previously


# [8.0.2] - 2021-2-16 [PR: #113](https://github.com/dolittle-entropy/vanir/pull/113)
## Summary

Gaining control over 3rd party dependencies.

### Fixed

- All third party dependencies are now locked to a specific version


# [8.0.1] - 2021-1-31 [PR: #106](https://github.com/dolittle-entropy/vanir/pull/106)
## Summary

Fixing a problem with the path for GraphQL client - needs a trailing /

### Fixed

- Added trailing slash for GraphQL paths for the client bindings


## [8.0.0] - 2021-01-24
### Added
- Changelog

### Changed
- Separator for environment variables have changed from single _ to double __.
  The separator is used for navigational purposes within the configuration object model.
- Restructuring documentation

## [7.1.7] - 2021-01-23
### Fixed
- Fixing reference to vanir.json and not config.json in the Dockerfile generated from template

## [7.1.6] - 2021-01-23
### Fixed
- Dropping `--env` options from scripts in `package.json` files in template to work with WebPack 5

## [7.1.5] - 2021-01-23
### Fixed
- Fixing generated Dockerfile to reflect what is in shared in the template

## [7.1.4] - 2021-01-23
### Added
- Non portal Typescript sample added

### Fixed
- Adding missing context resolution for Apollo setup (User-ID, Tenant-Id, Cookie)

## [7.1.3] - 2021-01-23
### Added
- C# and .NET Core support - early days - mostly investigation

### Fixed
- Fixing `microservice.scss` in template to correct for non-portal microservices and for portal ones
- Fixing lint errors in template

## [7.1.1 - 7.1.2] - 2021-01-16
### Fixed
- Exposing the publicPath property from configuration - this was missing

## [7.1.0] - 2021-01-16
### Fixed
- Fixing eventstore property in template

## [7.0.4] - 2021-01-16
### Fixed
- Lowercasing `eventstore` in Vanir configuration to make it possible to override with env variables. This should have bumped the major version.

## [7.0.3] - 2021-01-16
### Fixed
- Making package resolving more resilient and resolve to the correct path for `package.json` requires/imports in packages

## [7.0.2] - 2021-01-15
### Fixed
- Removing deprecated WebPack 5 options for the Terser plugin
- Setting correct option for production builds
- Fixing template for WebPack when generating microservices

## [7.0.1] - 2021-01-15
### Fixed
- Changing to deterministic for moduleIds for optimization - hashed is deprecated in WebPack 5
- Fixing how mjs files are resolved

## [7.0.0] - 2021-01-15
### Changed
- Upgrading to WebPack 5
- Dropping office-ui reference for SASS files and leverage @fluentui/react directly
- Updated all `package.json` files in templates to use the new way of running the WebPack devserver

### Added
- Added shebang loader for those dependencies that has a #! header on top of JS files

### Fixes
- Fixing sample dependencies to Vanir packages

## [6.5.1] - 2021-01-14
### Fixed
- Using `cross-env` for all scripts in `package.json` files to make developer experience work cross platform

## [6.5.0] - 2021-01-14
### Added
- Adding ESM replacement for yargs dependency to make WebPacking of backends work. MJS modules are resolved in a way that breaks the build ATM.

## [6.4.1] - 2021-01-12
### Fixed
- Fixing variable name to make configurable devserver port work

## [6.4.0] - 2021-01-12
### Added
- Ability to specify the WebPack devserver port from environment variable (port)

## [6.3.3] - 2021-01-11
### Fixed
- Making web option on Microservice an actual boolean in the generated file rather than a string

## [6.3.2] - 2021-01-10
### Fixed
- Switched to reading `application.json` from file instead of requiring it. Require for JSON files does not work at runtime after WebPacking

## [6.3.0 - 6.3.1] - 2021-01-10
### Added
- Flexibility around root, version and template paths for plop based generation

## [6.2.2] - 2021-01-10
### Fixed
- Basepath working cross-platform when generating from templates

## [6.2.1] - 2021-01-10
### Fixed
- Fix glob patterns for Globby to work in accordance with what is described in this issue: https://github.com/sindresorhus/globby/issues/155

## [6.2.0] - 2021-01-10
### Changed
- More robust handling of using the plopfile by requiring it rather than loading it. Enabling us to use the tooling in other scenarios where the file is not on disk but packaged inside with WebPack.

## [6.1.2] - 2021-01-09
### Fixed
- Adding `create-dolittle-microservice` as an actual dependency to `create-dolittle-app`- causing it to work in the wild :)

## [6.1.1] - 2021-01-08
### Fixed
- Fixing template for `microservice.json` file for route segment based on whether or not it is a portal creation

## [6.1.0] - 2021-01-08
### Added / Fixed
- Render portal related files when it is the portal being created during creation of microservices

## [6.0.2] - 2021-01-08
### Fixed
- Fixing route segment to be correct according to rooted or non-rooted microservices

### Added
- Adding a default portal template

## [6.0.1] - 2021-01-07
### Fixed
- Adding boolean to `RouteInfo` indicating whether or not it is an exact match.

## [6.0.0] - 2021-01-07
### Changed
- Changing `RouteInfo` to get the matched URL and the route information

## [5.0.0 - 5.1.0] - 2021-01-07
### Changed
- Changing from `paramsChanged()` to `routeChanged()` for consistency

## [4.3.0] - 2021-01-05
### Added
- Exporting `RouteInfo` from top level module

## [4.2.0] - 2021-01-05
### Added
- ViewModels can receive route info on `attached()` method

## [4.1.0] - 2021-01-05
### Added
- ViewModels can now implement a `paramsChanged()` method that gets called with route params

## [4.0.0] - 2021-01-05
### Changed
- References to shared components
- Public packages are now set to private in the templates

## [3.0.0] - 2021-01-05
### Changed
- Lifecycle manager for view models added
- Added sample

## [2.5.12] - 2021-01-02
### Fixed
- Exposing Web property on Microservice definition to indicate if a Microservice has a Web frontend or not

## [2.5.11] - 2021-01-01
### Fixed
- Fixing GraphQL route to guarantee having only a single slash

## [2.5.10] - 2021-01-01
### Fixed
- Fixing so that all templates are rendered with the current Vanir version as reference in package.json files
- Fixed output of the route segment to be correct dependeing on whether or not it is a portal or not

## [2.5.9] - 2021-01-01
### Fixed
- Hooking up TSOA out of the box

## [2.5.8] - 2021-01-01
### Fixed
- Making it possible to override the Web Dev Server port

## [2.5.7] - 2020-12-31
### Added
- Adding isRooted to the template for identifying wether or not a microservice is rooted in its URL strategy

## [2.5.6] - 2020-12-31
### Fixed
- Making sure the microservice Id is available at the right time during generation of applications with portal

## [2.5.5] - 2020-12-30
### Fixed
- Path for portal microservice.json file fixed during adding of portal from application creation

## [2.5.4] - 2020-12-30
### Fixed
- Using microservice id for identifying which Microservice is the portal

## [2.5.3] - 2020-12-30
### Fixed
- Passing target directory from application creation to micrsoervice creation when wanting a portal

## [2.5.2] - 2020-12-30
### Fixed
- Consistently using target directory throughout the code

## [2.5.1] - 2020-12-30
### Changed
- Consistently using the destination path throughout the code

## [2.5.0] - 2020-12-30
### Fixed
- References internally

## [2.4.0] - 2020-12-30
### Added
- Adding `useDialog()` hook for working more naturally with dialogs

## [2.3.2] - 2020-12-30
### Added
- Ability to generate to a preferred directory for the common plop helper

## [2.3.1] - 2020-12-29
### Changed
- Changing to vanir.json instead of config.json - should have caused a major release

### Added
- Preconfigured GitHub labels for semantic versioning for pull requests in our application template


## [2.3.0] - 2020-12-29
### Changes
- Templates refer to microservice.json instead of version.json

### Added
- Common types project


## [2.2.0] - 2020-12-29
### Changes
- Downgraded React to version 16 due to compatibility issues with other packages

### Added
- Proxy configuration in WebPack devserver configuration
- Adding application template files

### Fixed
- LINT fixes
- Removed copyright headers from templates
- Adding .hbs extension to package.json files in templates

## [2.1.11] - 2020-12-26
### Fixed
- Adding running of tests to CI scripts
- Fixing TSOA module replacement - used for building backends using WebPack
- Exporting nested modules for MVVM and routing in React package

## [2.1.10] - 2020-12-25
### Fixed
- Upgrading RXJS dependency

## [2.1.7 - 2.1.9] - 2020-12-25
### Fixed
- Fixing imports for deep modules that aren't exported at root
- Fixing imports to use the root exports from the Web package

## [2.1.6] - 2020-12-25
### Fixed
- Making children optional for RoutingProps

## [2.1.5] - 2020-12-25
### Fixed
- Cleaning up imports and exports to be accurate - not exporting from outside packages and such

## [2.1.4] - 2020-12-25
### Fixed
- Fixing content frame for setting the current document for messenger
- Fixing wrong imports
- Moving messenger and making things more decoupled

## [2.1.3] - 2020-12-25
### Fixed
- Fixing references for import statements - to work with packaging better
- Moving web configurations into correct location

## [2.1.2] - 2020-12-25
### Fixed
- Exporting more of the MVVM constructs at root module

## [2.1.1] - 2020-12-25
### Fixed
- Fixing dist folder in tsconfig.json files

## [2.1.0] - 2020-12-25
### Added
- Introducing basic routing and navigation to frontends

## [2.0.5] - 2020-12-25
### Fixed
- Exporting MVVM constructors from top level module

## [2.0.4] - 2020-12-25
### Fixed
- Adding types and main reference for package.json files

## [2.0.3] - 2020-12-25
### Changed
- Package name changed - this should have caused a major version number

## [2.0.2] - 2020-12-24
### Fixed
- Making WebPack dependencies regular dependencies - bubbling them up to consumers

## [2.0.1] - 2020-12-24
### Fixed
- Exposing logger and logging from top level module

## [2.0.0] - 2020-12-24
### Removed
- Elements that was brought in from other sources - changes the public API surface (tenants, microservices)

## [1.0.12] - 2020-12-24
### Fixed
- Adding source to packages

## [1.0.11] - 2020-12-24
### Fixed
- Adding missing metadata to packages

## [1.0.10] - 2020-12-24
### Fixed
- Fixing run scripts for build pipeline

## [1.0.9] - 2020-12-24
### Fixed
- Correcting metadata for all packages

## [1.0.8] - 2020-12-24
### Fixed
- Adding missing files in create-dolittle-app package
- Pulled in documentation from other sources

## [1.0.7] - 2020-12-24
### Fixed
- Making WebPack package public

## [1.0.6] - 2020-12-24
### Fixed
- Verbosity on publishing
- Engine strict to true

## [1.0.5] - 2020-12-24
### Fixed
- Node version 14 as minimum

## [1.0.4] - 2020-12-23
### Fixed
- Backend package

## [1.0.1] - [1.0.3] - 2020-12-23
### Fixed
- Adding ability to publish packages

## [1.0.0] - 2020-12-23 - Initial release
### Added
- Project scaffolding
- Crude WebPack setup for frontend
- Dependency Inversion setup with Tsyringe
- MVVM setup for React development
- Initial Plop templating
