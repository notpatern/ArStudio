using Newtonsoft.Json;

public class Demo {

}

public class DefaultDemo : Demo
{
    [JsonProperty("top-Left-Screen")]
    public string topLeftScreen { get; set; }

    [JsonProperty("top-Right-Screen")]
    public string topRightScreen { get; set; }

    [JsonProperty("bottom-Left-Screen")]
    public string bottomLeftScreen { get; set; }

    [JsonProperty("bottom-Right-Screen")]
    public string bottomRightScreen { get; set; }
}

public class ObsDemo : Demo
{
    public string ip { get; set; }
    public string port { get; set; }

    [JsonProperty("main-Screen")]
    public string mainScreen { get; set; }

    [JsonProperty("secondary-Screen")]
    public string secondaryScreen { get; set; }
}

public class Root
{
    [JsonProperty("default-demo")]
    public DefaultDemo defaultdemo { get; set; }

    [JsonProperty("volume-demo")]
    public VolumeDemo volumedemo { get; set; }

    [JsonProperty("obs-demo")]
    public ObsDemo obsdemo { get; set; }
}
public class VolumeDemo : Demo
{
    [JsonProperty("top-Left-Screen")]
    public string topLeftScreen { get; set; }

    [JsonProperty("top-Middle-Screen")]
    public string topMiddleScreen { get; set; }

    [JsonProperty("top-Right-Screen")]
    public string topRightScreen { get; set; }

    [JsonProperty("bottom-Left-Screen")]
    public string bottomLeftScreen { get; set; }

    [JsonProperty("bottom-Middle-Screen")]
    public string bottomMiddleScreen { get; set; }

    [JsonProperty("bottom-Right-Screen")]
    public string bottomRightScreen { get; set; }
}
