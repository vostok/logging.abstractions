## 1.0.9 (05.03.2020):

Fixed issue with `Guid?` properties logging.

## 1.0.8 (01.02.2020):

* Source context filtering extensions now treat supplied context values as case-insensitive prefixes.
* Added `EnrichBySourceContext` extension.

## 1.0.7 (23.01.2020):

* Added `WithMinimumLevelForSourceContext` extension.
* Added a public constructor with properties dictionary to `LogEvent`.

## 1.0.4 (08.11.2019):

Added `WithEventsSelectedBySourceContext` and `WithEventsDroppedBySourceContext` extensions.

## 1.0.3 (16.10.2019):

`LogEventExtensions` (`WithObjectProperties` and `WithParameters`) are public now.

## 1.0.2 (23.09.2019)

Fixed lowerCamelCase `WellKnownProperties`.

## 1.0.1 (15.03.2019)

* Added log levels transformation extension: `log.WithLevelsTransformation(...)`

## 1.0.0 (11.03.2019):

* Breaking change: source context is now hierarchical. Chained ForContext() calls are expected to accumulate a sequence of contexts instead of overriding current value.

* Introduced well-known property names for source context (resulting from ForContext calls), operation context (new name for contextual prefixes) and trace context.

* Introduced SourceContextValue and OperationContextValue classes as expected value types for properties mentioned above.

* LogEvent: added WithMessageTemplate(), WithException() and other similar methods.


## 0.1.5 (08.02.2019):

ILog extensions are now more forgiving to parameters count mismatch: provided params are now matched to names from template even when no exact match is found.


## 0.1.4 (01.02.2019):

Added `ILog.SelectEvents(Predicate<LogEvent>)` and `ILog.DropEvents(Predicate<LogEvent>)` extension methods.


## 0.1.3 (16.01.2019):

Added LogProvider: static shared configuration point for libraries.


## 0.1.2 (15.01.2019):

ForContext<T> extension no longer uses full type names by default.


## 0.1.1 (09.01.2019):

Precise timestamps in log events.


## 0.1.0 (06-09-2018): 

Initial prerelease.
