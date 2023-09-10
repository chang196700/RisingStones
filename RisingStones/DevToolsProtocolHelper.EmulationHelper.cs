using Microsoft.Web.WebView2.Core;

using System;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RisingStones
{
#nullable disable
    internal partial class DevToolsProtocolHelper
    {
        public class EmulationHelper
        {
            private CoreWebView2 coreWebView2;

            public EmulationHelper(CoreWebView2 coreWebView2)
            {
                this.coreWebView2 = coreWebView2;
            }

            //
            // 摘要:
            //     Screen orientation. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#type-ScreenOrientation
            public class ScreenOrientation
            {
                [JsonPropertyName("type")]
                public string Type { get; set; }

                [JsonPropertyName("angle")]
                public int Angle { get; set; }

                public ScreenOrientation()
                {
                }

                public ScreenOrientation(string type, int angle)
                {
                    Type = type;
                    Angle = angle;
                }
            }

            //
            // 摘要:
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#type-DisplayFeature
            public class DisplayFeature
            {
                [JsonPropertyName("orientation")]
                public string Orientation { get; set; }

                [JsonPropertyName("offset")]
                public int Offset { get; set; }

                [JsonPropertyName("maskLength")]
                public int MaskLength { get; set; }

                public DisplayFeature()
                {
                }

                public DisplayFeature(string orientation, int offset, int maskLength)
                {
                    Orientation = orientation;
                    Offset = offset;
                    MaskLength = maskLength;
                }
            }

            //
            // 摘要:
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#type-MediaFeature
            public class MediaFeature
            {
                [JsonPropertyName("name")]
                public string Name { get; set; }

                [JsonPropertyName("value")]
                public string Value { get; set; }

                public MediaFeature()
                {
                }

                public MediaFeature(string name, string value)
                {
                    Name = name;
                    Value = value;
                }
            }

            //
            // 摘要:
            //     Used to specify User Agent Cient Hints to emulate. See https://wicg.github.io/ua-client-hints
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#type-UserAgentBrandVersion
            public class UserAgentBrandVersion
            {
                [JsonPropertyName("brand")]
                public string Brand { get; set; }

                [JsonPropertyName("version")]
                public string Version { get; set; }

                public UserAgentBrandVersion()
                {
                }

                public UserAgentBrandVersion(string brand, string version)
                {
                    Brand = brand;
                    Version = version;
                }
            }

            //
            // 摘要:
            //     Used to specify User Agent Cient Hints to emulate. See https://wicg.github.io/ua-client-hints
            //     Missin g optional values will be filled in by the target with what it would normally
            //     use. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#type-UserAgentMetadata
            public class UserAgentMetadata
            {
                [JsonPropertyName("platform")]
                public string Platform { get; set; }

                [JsonPropertyName("platformVersion")]
                public string PlatformVersion { get; set; }

                [JsonPropertyName("architecture")]
                public string Architecture { get; set; }

                [JsonPropertyName("model")]
                public string Model { get; set; }

                [JsonPropertyName("mobile")]
                public bool Mobile { get; set; }

                [JsonPropertyName("brands")]
                public UserAgentBrandVersion[] Brands { get; set; }

                [JsonPropertyName("fullVersion")]
                public string FullVersion { get; set; }

                public UserAgentMetadata()
                {
                }

                public UserAgentMetadata(string platform, string platformVersion, string architecture, string model, bool mobile, UserAgentBrandVersion[] brands = null, string fullVersion = null)
                {
                    Platform = platform;
                    PlatformVersion = platformVersion;
                    Architecture = architecture;
                    Model = model;
                    Mobile = mobile;
                    Brands = brands;
                    FullVersion = fullVersion;
                }
            }

            public class VirtualTimeBudgetExpiredEventArgs : EventArgs
            {
            }

            private EventHandler<VirtualTimeBudgetExpiredEventArgs> virtualTimeBudgetExpired;

            private bool virtualTimeBudgetExpiredFlag;

            //
            // 摘要:
            //     Notification sent after the virtual time budget for the current VirtualTimePolicy
            //     has run out. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#event-virtualTimeBudgetExpired
            public event EventHandler<VirtualTimeBudgetExpiredEventArgs> VirtualTimeBudgetExpired
            {
                add
                {
                    virtualTimeBudgetExpired = (EventHandler<VirtualTimeBudgetExpiredEventArgs>)Delegate.Combine(virtualTimeBudgetExpired, value);
                    if (!virtualTimeBudgetExpiredFlag)
                    {
                        coreWebView2.GetDevToolsProtocolEventReceiver("Emulation.virtualTimeBudgetExpired").DevToolsProtocolEventReceived += RaiseVirtualTimeBudgetExpired;
                        virtualTimeBudgetExpiredFlag = true;
                    }
                }
                remove
                {
                    virtualTimeBudgetExpired = (EventHandler<VirtualTimeBudgetExpiredEventArgs>)Delegate.Remove(virtualTimeBudgetExpired, value);
                    if (virtualTimeBudgetExpired == null)
                    {
                        coreWebView2.GetDevToolsProtocolEventReceiver("Emulation.virtualTimeBudgetExpired").DevToolsProtocolEventReceived -= RaiseVirtualTimeBudgetExpired;
                    }
                }
            }

            //
            // 摘要:
            //     Tells whether emulation is supported. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-canEmulate
            public async Task<bool> CanEmulateAsync()
            {
                return JsonDocument.Parse(await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.canEmulate", "{}")).RootElement.EnumerateObject().First().Value.GetBoolean();
            }

            //
            // 摘要:
            //     Clears the overriden device metrics. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-clearDeviceMetricsOverride
            public async Task ClearDeviceMetricsOverrideAsync()
            {
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.clearDeviceMetricsOverride", "{}");
            }

            //
            // 摘要:
            //     Clears the overriden Geolocation Position and Error. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-clearGeolocationOverride
            public async Task ClearGeolocationOverrideAsync()
            {
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.clearGeolocationOverride", "{}");
            }

            //
            // 摘要:
            //     Requests that page scale factor is reset to initial values. CDP Documentation:
            //     https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-resetPageScaleFactor
            public async Task ResetPageScaleFactorAsync()
            {
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.resetPageScaleFactor", "{}");
            }

            //
            // 摘要:
            //     Enables or disables simulating a focused and active page. CDP Documentation:
            //     https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setFocusEmulationEnabled
            public async Task SetFocusEmulationEnabledAsync(bool enabled)
            {
                dynamic val = new ExpandoObject();
                val.enabled = enabled;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setFocusEmulationEnabled", parametersAsJson);
            }

            //
            // 摘要:
            //     Enables CPU throttling to emulate slow CPUs. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setCPUThrottlingRate
            public async Task SetCPUThrottlingRateAsync(double rate)
            {
                dynamic val = new ExpandoObject();
                val.rate = rate;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setCPUThrottlingRate", parametersAsJson);
            }

            //
            // 摘要:
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setScrollbarsHidden
            public async Task SetScrollbarsHiddenAsync(bool hidden)
            {
                dynamic val = new ExpandoObject();
                val.hidden = hidden;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setScrollbarsHidden", parametersAsJson);
            }

            //
            // 摘要:
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setDocumentCookieDisabled
            public async Task SetDocumentCookieDisabledAsync(bool disabled)
            {
                dynamic val = new ExpandoObject();
                val.disabled = disabled;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setDocumentCookieDisabled", parametersAsJson);
            }

            //
            // 摘要:
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setEmitTouchEventsForMouse
            public async Task SetEmitTouchEventsForMouseAsync(bool enabled, string configuration = null)
            {
                dynamic val = new ExpandoObject();
                val.enabled = enabled;
                if (configuration != null)
                {
                    val.configuration = configuration;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setEmitTouchEventsForMouse", parametersAsJson);
            }

            //
            // 摘要:
            //     Emulates the given media type or media feature for CSS media queries. CDP Documentation:
            //     https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setEmulatedMedia
            public async Task SetEmulatedMediaAsync(string media = null, MediaFeature[] features = null)
            {
                dynamic val = new ExpandoObject();
                if (media != null)
                {
                    val.media = media;
                }

                if (features != null)
                {
                    val.features = features;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setEmulatedMedia", parametersAsJson);
            }

            //
            // 摘要:
            //     Emulates the given vision deficiency. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setEmulatedVisionDeficiency
            public async Task SetEmulatedVisionDeficiencyAsync(string type)
            {
                dynamic val = new ExpandoObject();
                val.type = type;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setEmulatedVisionDeficiency", parametersAsJson);
            }

            //
            // 摘要:
            //     Overrides the Geolocation Position or Error. Omitting any of the parameters emulates
            //     position unavai lable. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setGeolocationOverride
            public async Task SetGeolocationOverrideAsync(double? latitude = null, double? longitude = null, double? accuracy = null)
            {
                dynamic val = new ExpandoObject();
                if (latitude.HasValue)
                {
                    val.latitude = latitude;
                }

                if (longitude.HasValue)
                {
                    val.longitude = longitude;
                }

                if (accuracy.HasValue)
                {
                    val.accuracy = accuracy;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setGeolocationOverride", parametersAsJson);
            }

            //
            // 摘要:
            //     Overrides the Idle state. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setIdleOverride
            public async Task SetIdleOverrideAsync(bool isUserActive, bool isScreenUnlocked)
            {
                dynamic val = new ExpandoObject();
                val.isUserActive = isUserActive;
                val.isScreenUnlocked = isScreenUnlocked;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setIdleOverride", parametersAsJson);
            }

            //
            // 摘要:
            //     Clears Idle state overrides. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-clearIdleOverride
            public async Task ClearIdleOverrideAsync()
            {
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.clearIdleOverride", "{}");
            }

            //
            // 摘要:
            //     Overrides value returned by the javascript navigator object. CDP Documentation:
            //     https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setNavigatorOverrides
            public async Task SetNavigatorOverridesAsync(string platform)
            {
                dynamic val = new ExpandoObject();
                val.platform = platform;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setNavigatorOverrides", parametersAsJson);
            }

            //
            // 摘要:
            //     Sets a specified page scale factor. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setPageScaleFactor
            public async Task SetPageScaleFactorAsync(double pageScaleFactor)
            {
                dynamic val = new ExpandoObject();
                val.pageScaleFactor = pageScaleFactor;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setPageScaleFactor", parametersAsJson);
            }

            //
            // 摘要:
            //     Switches script execution in the page. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setScriptExecutionDisabled
            public async Task SetScriptExecutionDisabledAsync(bool value)
            {
                dynamic val = new ExpandoObject();
                val.value = value;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setScriptExecutionDisabled", parametersAsJson);
            }

            //
            // 摘要:
            //     Enables touch on platforms which do not support them. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setTouchEmulationEnabled
            public async Task SetTouchEmulationEnabledAsync(bool enabled, int? maxTouchPoints = null)
            {
                dynamic val = new ExpandoObject();
                val.enabled = enabled;
                if (maxTouchPoints.HasValue)
                {
                    val.maxTouchPoints = maxTouchPoints;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setTouchEmulationEnabled", parametersAsJson);
            }

            //
            // 摘要:
            //     Turns on virtual time for all frames (replacing real-time with a synthetic time
            //     source) and sets the current virtual time policy. Note this supersedes any previous
            //     time budget. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setVirtualTimePolicy
            public async Task<double> SetVirtualTimePolicyAsync(string policy, double? budget = null, int? maxVirtualTimeTaskStarvationCount = null, bool? waitForNavigation = null, double? initialVirtualTime = null)
            {
                dynamic val = new ExpandoObject();
                val.policy = policy;
                if (budget.HasValue)
                {
                    val.budget = budget;
                }

                if (maxVirtualTimeTaskStarvationCount.HasValue)
                {
                    val.maxVirtualTimeTaskStarvationCount = maxVirtualTimeTaskStarvationCount;
                }

                if (waitForNavigation.HasValue)
                {
                    val.waitForNavigation = waitForNavigation;
                }

                if (initialVirtualTime.HasValue)
                {
                    val.initialVirtualTime = initialVirtualTime;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                return JsonDocument.Parse(await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setVirtualTimePolicy", parametersAsJson)).RootElement.EnumerateObject().First().Value.GetDouble();
            }

            //
            // 摘要:
            //     Overrides default host system locale with the specified one. CDP Documentation:
            //     https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setLocaleOverride
            public async Task SetLocaleOverrideAsync(string locale = null)
            {
                dynamic val = new ExpandoObject();
                if (locale != null)
                {
                    val.locale = locale;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setLocaleOverride", parametersAsJson);
            }

            //
            // 摘要:
            //     Overrides default host system timezone with the specified one. CDP Documentation:
            //     https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setTimezoneOverride
            public async Task SetTimezoneOverrideAsync(string timezoneId)
            {
                dynamic val = new ExpandoObject();
                val.timezoneId = timezoneId;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setTimezoneOverride", parametersAsJson);
            }

            //
            // 摘要:
            //     Resizes the frame/viewport of the page. Note that this does not affect the frame's
            //     container (e.g. b rowser window). Can be used to produce screenshots of the specified
            //     size. Not supported on Android. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setVisibleSize
            public async Task SetVisibleSizeAsync(int width, int height)
            {
                dynamic val = new ExpandoObject();
                val.width = width;
                val.height = height;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setVisibleSize", parametersAsJson);
            }

            //
            // 摘要:
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setDisabledImageTypes
            public async Task SetDisabledImageTypesAsync(string[] imageTypes)
            {
                dynamic val = new ExpandoObject();
                val.imageTypes = imageTypes;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setDisabledImageTypes", parametersAsJson);
            }

            //
            // 摘要:
            //     Allows overriding user agent with the given string. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Emulation/#method-setUserAgentOverride
            public async Task SetUserAgentOverrideAsync(string userAgent, string acceptLanguage = null, string platform = null, UserAgentMetadata userAgentMetadata = null)
            {
                dynamic val = new ExpandoObject();
                val.userAgent = userAgent;
                if (acceptLanguage != null)
                {
                    val.acceptLanguage = acceptLanguage;
                }

                if (platform != null)
                {
                    val.platform = platform;
                }

                if (userAgentMetadata != null)
                {
                    val.userAgentMetadata = userAgentMetadata;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setUserAgentOverride", parametersAsJson);
            }

            private void RaiseVirtualTimeBudgetExpired(object sender, CoreWebView2DevToolsProtocolEventReceivedEventArgs args)
            {
                VirtualTimeBudgetExpiredEventArgs e = JsonSerializer.Deserialize<VirtualTimeBudgetExpiredEventArgs>(args.ParameterObjectAsJson);
                virtualTimeBudgetExpired?.Invoke(this, e);
            }
        }
    }
}
