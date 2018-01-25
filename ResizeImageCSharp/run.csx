#r "Microsoft.WindowsAzure.Storage"

using ImageResizer;

public static void Run(
    Stream image,                           // input blob, large size
    string name,
    Stream imageSmall,
    Stream imageMedium, TraceWriter log)  // output blobs
{
    var imageBuilder = ImageResizer.ImageBuilder.Current;
    var size = imageDimensionsTable[ImageSize.Small];

    log.Info($"C# Blob trigger function processing blob\n Name:{name} \n Size: {image.Length} Bytes");

    imageBuilder.Build(
        image, imageSmall,
        new ResizeSettings(size.Item1, size.Item2, FitMode.Max, null), false);

    log.Info($"C# Blob trigger function scaled small image\n Name:{name}");

    image.Position = 0;
    size = imageDimensionsTable[ImageSize.Medium];

    imageBuilder.Build(
        image, imageMedium,
        new ResizeSettings(size.Item1, size.Item2, FitMode.Max, null), false);
    log.Info($"C# Blob trigger function scaled medium image\n Name:{name}");
    
}

public enum ImageSize
{
    ExtraSmall, Small, Medium
}

private static Dictionary<ImageSize, Tuple<int, int>> imageDimensionsTable = new Dictionary<ImageSize, Tuple<int, int>>()
        {
            { ImageSize.ExtraSmall, Tuple.Create(320, 200) },
            { ImageSize.Small,      Tuple.Create(640, 400) },
            { ImageSize.Medium,     Tuple.Create(800, 600) }
        };
