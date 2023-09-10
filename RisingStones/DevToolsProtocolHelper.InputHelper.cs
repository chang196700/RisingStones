using Microsoft.Web.WebView2.Core;

using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RisingStones
{
#nullable disable
    internal partial class DevToolsProtocolHelper
    {
        public class InputHelper
        {

            //
            // 摘要:
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#type-TouchPoint
            public class TouchPoint
            {
                [JsonPropertyName("x")]
                public double X { get; set; }

                [JsonPropertyName("y")]
                public double Y { get; set; }

                [JsonPropertyName("radiusX")]
                public double? RadiusX { get; set; }

                [JsonPropertyName("radiusY")]
                public double? RadiusY { get; set; }

                [JsonPropertyName("rotationAngle")]
                public double? RotationAngle { get; set; }

                [JsonPropertyName("force")]
                public double? Force { get; set; }

                [JsonPropertyName("tangentialPressure")]
                public double? TangentialPressure { get; set; }

                [JsonPropertyName("tiltX")]
                public int? TiltX { get; set; }

                [JsonPropertyName("tiltY")]
                public int? TiltY { get; set; }

                [JsonPropertyName("twist")]
                public int? Twist { get; set; }

                [JsonPropertyName("id")]
                public double? Id { get; set; }

                public TouchPoint()
                {
                }

                public TouchPoint(double x, double y, double? radiusX = null, double? radiusY = null, double? rotationAngle = null, double? force = null, double? tangentialPressure = null, int? tiltX = null, int? tiltY = null, int? twist = null, double? id = null)
                {
                    X = x;
                    Y = y;
                    RadiusX = radiusX;
                    RadiusY = radiusY;
                    RotationAngle = rotationAngle;
                    Force = force;
                    TangentialPressure = tangentialPressure;
                    TiltX = tiltX;
                    TiltY = tiltY;
                    Twist = twist;
                    Id = id;
                }
            }

            private CoreWebView2 coreWebView2;

            public InputHelper(CoreWebView2 coreWebView2)
            {
                this.coreWebView2 = coreWebView2;
            }

            //
            // 摘要:
            //     Dispatches a key event to the page. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-dispatchKeyEvent
            public async Task DispatchKeyEventAsync(string type, int? modifiers = null, double? timestamp = null, string text = null, string unmodifiedText = null, string keyIdentifier = null, string code = null, string key = null, int? windowsVirtualKeyCode = null, int? nativeVirtualKeyCode = null, bool? autoRepeat = null, bool? isKeypad = null, bool? isSystemKey = null, int? location = null, string[] commands = null)
            {
                dynamic val = new ExpandoObject();
                val.type = type;
                if (modifiers.HasValue)
                {
                    val.modifiers = modifiers;
                }

                if (timestamp.HasValue)
                {
                    val.timestamp = timestamp;
                }

                if (text != null)
                {
                    val.text = text;
                }

                if (unmodifiedText != null)
                {
                    val.unmodifiedText = unmodifiedText;
                }

                if (keyIdentifier != null)
                {
                    val.keyIdentifier = keyIdentifier;
                }

                if (code != null)
                {
                    val.code = code;
                }

                if (key != null)
                {
                    val.key = key;
                }

                if (windowsVirtualKeyCode.HasValue)
                {
                    val.windowsVirtualKeyCode = windowsVirtualKeyCode;
                }

                if (nativeVirtualKeyCode.HasValue)
                {
                    val.nativeVirtualKeyCode = nativeVirtualKeyCode;
                }

                if (autoRepeat.HasValue)
                {
                    val.autoRepeat = autoRepeat;
                }

                if (isKeypad.HasValue)
                {
                    val.isKeypad = isKeypad;
                }

                if (isSystemKey.HasValue)
                {
                    val.isSystemKey = isSystemKey;
                }

                if (location.HasValue)
                {
                    val.location = location;
                }

                if (commands != null)
                {
                    val.commands = commands;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.dispatchKeyEvent", parametersAsJson);
            }

            //
            // 摘要:
            //     This method emulates inserting text that doesn't come from a key press, for example
            //     an emoji keyboar d or an IME. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-insertText
            public async Task InsertTextAsync(string text)
            {
                dynamic val = new ExpandoObject();
                val.text = text;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.insertText", parametersAsJson);
            }

            //
            // 摘要:
            //     Dispatches a mouse event to the page. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-dispatchMouseEvent
            public async Task DispatchMouseEventAsync(string type, double x, double y, int? modifiers = null, double? timestamp = null, string button = null, int? buttons = null, int? clickCount = null, double? force = null, double? tangentialPressure = null, int? tiltX = null, int? tiltY = null, int? twist = null, double? deltaX = null, double? deltaY = null, string pointerType = null)
            {
                dynamic val = new ExpandoObject();
                val.type = type;
                val.x = x;
                val.y = y;
                if (modifiers.HasValue)
                {
                    val.modifiers = modifiers;
                }

                if (timestamp.HasValue)
                {
                    val.timestamp = timestamp;
                }

                if (button != null)
                {
                    val.button = button;
                }

                if (buttons.HasValue)
                {
                    val.buttons = buttons;
                }

                if (clickCount.HasValue)
                {
                    val.clickCount = clickCount;
                }

                if (force.HasValue)
                {
                    val.force = force;
                }

                if (tangentialPressure.HasValue)
                {
                    val.tangentialPressure = tangentialPressure;
                }

                if (tiltX.HasValue)
                {
                    val.tiltX = tiltX;
                }

                if (tiltY.HasValue)
                {
                    val.tiltY = tiltY;
                }

                if (twist.HasValue)
                {
                    val.twist = twist;
                }

                if (deltaX.HasValue)
                {
                    val.deltaX = deltaX;
                }

                if (deltaY.HasValue)
                {
                    val.deltaY = deltaY;
                }

                if (pointerType != null)
                {
                    val.pointerType = pointerType;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.dispatchMouseEvent", parametersAsJson);
            }

            //
            // 摘要:
            //     Dispatches a touch event to the page. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-dispatchTouchEvent
            public async Task DispatchTouchEventAsync(string type, TouchPoint[] touchPoints, int? modifiers = null, double? timestamp = null)
            {
                dynamic val = new ExpandoObject();
                val.type = type;
                val.touchPoints = touchPoints;
                if (modifiers.HasValue)
                {
                    val.modifiers = modifiers;
                }

                if (timestamp.HasValue)
                {
                    val.timestamp = timestamp;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.dispatchTouchEvent", parametersAsJson);
            }

            //
            // 摘要:
            //     Emulates touch event from the mouse event parameters. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-emulateTouchFromMouseEvent
            public async Task EmulateTouchFromMouseEventAsync(string type, int x, int y, string button, double? timestamp = null, double? deltaX = null, double? deltaY = null, int? modifiers = null, int? clickCount = null)
            {
                dynamic val = new ExpandoObject();
                val.type = type;
                val.x = x;
                val.y = y;
                val.button = button;
                if (timestamp.HasValue)
                {
                    val.timestamp = timestamp;
                }

                if (deltaX.HasValue)
                {
                    val.deltaX = deltaX;
                }

                if (deltaY.HasValue)
                {
                    val.deltaY = deltaY;
                }

                if (modifiers.HasValue)
                {
                    val.modifiers = modifiers;
                }

                if (clickCount.HasValue)
                {
                    val.clickCount = clickCount;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.emulateTouchFromMouseEvent", parametersAsJson);
            }

            //
            // 摘要:
            //     Ignores input events (useful while auditing page). CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-setIgnoreInputEvents
            public async Task SetIgnoreInputEventsAsync(bool ignore)
            {
                dynamic val = new ExpandoObject();
                val.ignore = ignore;
                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.setIgnoreInputEvents", parametersAsJson);
            }

            //
            // 摘要:
            //     Synthesizes a pinch gesture over a time period by issuing appropriate touch events.
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-synthesizePinchGesture
            public async Task SynthesizePinchGestureAsync(double x, double y, double scaleFactor, int? relativeSpeed = null, string gestureSourceType = null)
            {
                dynamic val = new ExpandoObject();
                val.x = x;
                val.y = y;
                val.scaleFactor = scaleFactor;
                if (relativeSpeed.HasValue)
                {
                    val.relativeSpeed = relativeSpeed;
                }

                if (gestureSourceType != null)
                {
                    val.gestureSourceType = gestureSourceType;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.synthesizePinchGesture", parametersAsJson);
            }

            //
            // 摘要:
            //     Synthesizes a scroll gesture over a time period by issuing appropriate touch
            //     events. CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-synthesizeScrollGesture
            public async Task SynthesizeScrollGestureAsync(double x, double y, double? xDistance = null, double? yDistance = null, double? xOverscroll = null, double? yOverscroll = null, bool? preventFling = null, int? speed = null, string gestureSourceType = null, int? repeatCount = null, int? repeatDelayMs = null, string interactionMarkerName = null)
            {
                dynamic val = new ExpandoObject();
                val.x = x;
                val.y = y;
                if (xDistance.HasValue)
                {
                    val.xDistance = xDistance;
                }

                if (yDistance.HasValue)
                {
                    val.yDistance = yDistance;
                }

                if (xOverscroll.HasValue)
                {
                    val.xOverscroll = xOverscroll;
                }

                if (yOverscroll.HasValue)
                {
                    val.yOverscroll = yOverscroll;
                }

                if (preventFling.HasValue)
                {
                    val.preventFling = preventFling;
                }

                if (speed.HasValue)
                {
                    val.speed = speed;
                }

                if (gestureSourceType != null)
                {
                    val.gestureSourceType = gestureSourceType;
                }

                if (repeatCount.HasValue)
                {
                    val.repeatCount = repeatCount;
                }

                if (repeatDelayMs.HasValue)
                {
                    val.repeatDelayMs = repeatDelayMs;
                }

                if (interactionMarkerName != null)
                {
                    val.interactionMarkerName = interactionMarkerName;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.synthesizeScrollGesture", parametersAsJson);
            }

            //
            // 摘要:
            //     Synthesizes a tap gesture over a time period by issuing appropriate touch events.
            //     CDP Documentation: https://chromedevtools.github.io/devtools-protocol/tot/Input/#method-synthesizeTapGesture
            public async Task SynthesizeTapGestureAsync(double x, double y, int? duration = null, int? tapCount = null, string gestureSourceType = null)
            {
                dynamic val = new ExpandoObject();
                val.x = x;
                val.y = y;
                if (duration.HasValue)
                {
                    val.duration = duration;
                }

                if (tapCount.HasValue)
                {
                    val.tapCount = tapCount;
                }

                if (gestureSourceType != null)
                {
                    val.gestureSourceType = gestureSourceType;
                }

                string parametersAsJson = JsonSerializer.Serialize<object>(val);
                await coreWebView2.CallDevToolsProtocolMethodAsync("Input.synthesizeTapGesture", parametersAsJson);
            }
        }
    }
}
