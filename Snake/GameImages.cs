namespace Snake;

internal static class GameImages
{
    private const string PathPrefix = $"../../../Images";

    public static readonly Dictionary<Cell, Image> mapImages = new()
    {
        [Cell.Empty] = Image.FromFile($"{PathPrefix}/Empty.jpg"),
        [Cell.Wall] = Image.FromFile($"{PathPrefix}/Wall.jpg"),
        [Cell.Food] = Image.FromFile($"{PathPrefix}/Food.jpg"),
        [Cell.Defence] = Image.FromFile($"{PathPrefix}/Defence.jpg"),
        [Cell.SharpTeeth] = Image.FromFile($"{PathPrefix}/SharpTeeth.png"),
        [Cell.MagicClock] = Image.FromFile($"{PathPrefix}/MagicClock.jpg"),
        [Cell.Snake] = Image.FromFile($"{PathPrefix}/BodySnake.png"),
        [Cell.ReverseItem] = Image.FromFile($"{PathPrefix}/ReverseItem.jpg")
    };

    public static readonly Dictionary<Cell, Image> invetoryImages = new()
    {
        [Cell.Defence] = Image.FromFile($"{PathPrefix}/DefenceWhite.jpg"),
        [Cell.SharpTeeth] = Image.FromFile($"{PathPrefix}/SharpTeethWhite.png"),
        [Cell.MagicClock] = Image.FromFile($"{PathPrefix}/MagicClockWhite.jpg"),
        [Cell.ReverseItem] = Image.FromFile($"{PathPrefix}/ReverseItemWhite.jpg")
    };

    public static readonly Dictionary<GameAction, Image> headImages = new()
    {
        [GameAction.Up] = Image.FromFile($"{PathPrefix}/Head.png"),
        [GameAction.Down] = RotateImage(Image.FromFile($"{PathPrefix}/Head.png"), RotateFlipType.RotateNoneFlipY),
        [GameAction.Left] = RotateImage(Image.FromFile($"{PathPrefix}/Head.png"), RotateFlipType.Rotate270FlipNone),
        [GameAction.Right] = RotateImage(Image.FromFile($"{PathPrefix}/Head.png"), RotateFlipType.Rotate90FlipNone)
    };

    private static Image RotateImage(Image image, RotateFlipType rotateType)
    {
        image.RotateFlip(rotateType);
        return image;
    }
}