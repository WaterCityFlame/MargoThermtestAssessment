# MargoThermtestAssessment
Margo Collins' attempt at the software_engineer_test from Thermtest

This is a simple C# and .NET app that displays temperature data from a CSV file as through it were being streamed in from a test device. Noise filtering is applied to the data.

## Assumptions Made:

  1. **detect temperature becoming stable around a set point**

    In real life, I would ask an engineer or data scientist for a good algorithm for this. Since that's not really an option at the moment, I've decided on a very simple algorithm: the temperature has become stable if the rolling standard deviation is less than 1. Realistically, if temperature is varying by less than 1 degree, I'd view that as stable. It looks right on the UI.

  2. **average temperature and RSD displayed over a period of 10s**

    Not sure if you wanted to see this on the graph or as text to the side. You get both!

  3. **the ability to save the test results as an XML document**

    The XML document will only save the results that have been displayed so far. It's like the results not displayed are from timestamps in the future and cannot be displayed.

  4. **the file should be selectable**

    It is ("Open File" button, right sidebar). But! The test file that you sent me is included with this project & is open and graphed by default. Good apps give *instant* results :D

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
