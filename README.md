# MargoThermtestAssessment
Margo Collins' attempt at the software_engineer_test from Thermtest

This is a simple C# and .NET app that displays temperature data from a CSV file as through it were being streamed in from a test device. Noise filtering is applied to the data. 

---
## The original assignment description is as follows: 

Software Engineering Practical Assessment:

This assessment is based on your knowledge of the following concepts : 

 - .NET Framework
 - C#
 - WPF
 - XML Serialization
 - Oxyplot
 - MVC/MVVM, a super-loop implementation of this project might seem tempeting due to it's relative simplicity.

Attached is a a data stream captured by a temperature controllers. The temperature controller is attempting to stabilize a 
plant around a set-point.

It is formatted as follows:

<Temperature in degree C>,<Timestamp>

The file should be selectable, and each new line treated as a new temperature sample.

Each sample must be displayed, polled and displayed at a fix sample rate of 200ms, and displayed on
the graph using the timestamp provided.

The data should appear as if the data was being polled at the fixed sample rate in real-time.

While being graphed:

- the data must be filtered of noise ( any filter of your choice, a simple FIR/moving average just to get rid of gaussian noise).
- the average temperature and RSD displayed over a period of 10s. Displayed in real time while the test is running.
- The ability to detect temperature becoming stable around a set point
- Having the user the ability to save the test results as an XML document with the format of your choice.
