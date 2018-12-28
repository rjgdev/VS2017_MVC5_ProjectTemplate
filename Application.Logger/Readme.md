# How to use
This document will show you how to can use this Logger.


### Methods
_log.Info()  
_log.Debug()  
_log.Error()  
_Log.Fatal()  
_Log.Warn()  


### Step 1
Insert in the last line of AssemblyInfo.cs file

```xml
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
```

### Step 2
Declaring the ***_Log*** variable.
```csharp
using log4net;
public class CollegeDetailsController : ApiController
{
    private readonly ILog _log = Logger.LoggingInstance;
}
```

### Step 3

Install the nuget package [log4net version 2.0.8](https://www.nuget.org/packages/log4net/2.0.8)   

The configuration below will instruct the log4net application to save the logs into the database.


Copy the below text into the Web.config file of the referring project.  
```xml
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler" />
  </configSections>
  <log4net debug="true">
    <appender name="ADONetAppender" type="log4net.Appender.ADONetAppender">
      <bufferSize value="1" />
      <connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
      <connectionString value="data source=.\SQLEXPRESS;initial catalog=WmsLiteDB;integrated security=false;persist security info=True;User ID=sa;Password=P@$$w0rd" />
      <commandText value="INSERT INTO Logs ([Date],[Thread],[Level],[Logger],[Message],[Exception]) VALUES
       (@log_date, @thread, @log_level, @logger, @message, @exception)" />
      <parameter>
        <parameterName value="@log_date" />
        <dbType value="DateTime" />
        <layout type="log4net.Layout.RawTimeStampLayout" />
      </parameter>
      <parameter>
        <parameterName value="@thread" />
        <dbType value="String" />
        <size value="32" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%t" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@log_level" />
        <dbType value="String" />
        <size value="512" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%p" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@logger" />
        <dbType value="String" />
        <size value="512" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%c" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@message" />
        <dbType value="String" />
        <size value="4000" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%m" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@exception" />
        <dbType value="String" />
        <size value="2000" />
        <layout type="log4net.Layout.ExceptionLayout" />
      </parameter>
    </appender>
    <root>
      <level value="DEBUG" />
      <appender-ref ref="ADONetAppender" />
    </root>
  </log4net>
  </configuration>
```