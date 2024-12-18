## 1.0.33 (12-12-2024): 

Bump NuGet deps versions

## 1.0.32 (24-11-2023):

Minor optimizations

## 1.0.31 (01-02-2023):

- Stopped writing interpolated properties with invalid names
- Added global `LogExtensions_Interpolated.Enabled` setting

## 1.0.30 (03-03-2022):

Added `Properties` field to OperationContextValue.

## 1.0.29 (26-01-2022):

Added escaping for interpolated property names.

## 1.0.28 (13-01-2022):

Reduced memory traffic in log extensoins, added several optimizations.

## 1.0.27 (10-01-2022):

Added interpolated string handlers.

## 1.0.26 (30-12-2021):

WithMutatedPropertiesLog minor optimization

## 1.0.25 (27-12-2021):

Speed up Info\Error\etc extensions by ~10-20 percent.

## 1.0.24 (24-12-2021):

Added `WithTransformation` extention.

## 1.0.23 (06-12-2021):

Added `net6.0` target.

## 1.0.21 (11.06.2021):

Allow dot character in property names.

## 1.0.20 (20.02.2021):

WithMinimumLevel can take minLevelProvider of type Func<LogLevel> now.

## 1.0.19 (30.12.2020):

ForContext now offers nice formatting for generic type names.

## 1.0.18 (24.08.2020):

Filters by source contexts now use `SourceContextWrapper`.

## 1.0.17 (25.06.2020):

- Moderate performance improvements (up to 30% more single-threaded throughput);
- Outer WithProperty decorators are now able to overwrite properties defined by inner ones.

## 1.0.15 (13.06.2020):

Bugfix in DeconstructionHelper: anonymous types may have a custom ToString() implementation.

## 1.0.14 (06.06.2020):

Fixed https://github.com/vostok/logging.abstractions/issues/13.

The rules for deconstruction of the single argument are now as follows (in the order of priority):

- Always deconstruct IReadonlyDictionary<string, object>
- Do not deconstruct any other collections
- Do not deconstruct types with custom ToString() implementation
- Always deconstruct anonymous types
- Always deconstruct when message template has more than one property
- Do not deconstruct anything else

## 1.0.13 (06.06.2020):

Implemented https://github.com/vostok/logging.abstractions/issues/10

## 1.0.12 (30.05.2020):

Minor fix in detection of anonymous types.

## 1.0.11 (26.05.2020):

Added `WithErrorsTransformedToWarns` extension.

## 1.0.10 (26.05.2020):

Fixed https://github.com/vostok/logging.abstractions/issues/11

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
