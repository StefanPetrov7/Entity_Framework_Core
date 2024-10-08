namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            // => Task 2
            //int producerId = int.Parse(Console.ReadLine());
            //string resultOne = ExportAlbumsInfo(context, producerId);
            //Console.WriteLine(resultOne);

            // => Task 3
            int duration = int.Parse(Console.ReadLine());
            string resultTwo = ExportSongsAboveDuration(context, duration);
            Console.WriteLine(resultTwo);

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var query = context.Albums.Where(x => x.ProducerId == producerId)
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = x.Producer.Name,
                    AlbumPrice = x.Price,
                    Songs = x.Songs.Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Price,
                        WriterName = x.Writer.Name
                    }).OrderByDescending(x => x.Name).ThenBy(x => x.WriterName).ToList(),
                })
                .AsEnumerable()
                .OrderByDescending(x => x.AlbumPrice)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var albums in query)
            {
                result.AppendLine($"-AlbumName: {albums.AlbumName}");
                result.AppendLine($"-ReleaseDate: {albums.ReleaseDate}");
                result.AppendLine($"-ProducerName: {albums.ProducerName}");
                result.AppendLine($"-Songs:");

                int counter = 0;

                foreach (var song in albums.Songs)
                {
                    result.AppendLine($"---#{++counter}");
                    result.AppendLine($"---SongName: {song.Name}");
                    result.AppendLine($"---Price: {song.Price:F2}");
                    result.AppendLine($"---Writer: {song.WriterName}");
                }

                result.AppendLine($"-AlbumPrice: {albums.AlbumPrice:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var query = context.Songs.Where(x => (x.Duration.Minutes * 60 + x.Duration.Seconds) > duration)
                .Select(x => new
                {
                    SongName = x.Name,
                    WriterName = x.Writer.Name,
                    AlbumProducer = x.Album.Producer.Name,
                    x.Duration,
                    Performers = x.SongPerformers.Select(x => new
                    {
                        FullName = x.Performer.FirstName + " " + x.Performer.LastName,
                    })
                    .OrderBy(x => x.FullName).ToList(),
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.WriterName)
                .ToList();

            StringBuilder result = new StringBuilder();

            int counter = 0;

            foreach (var song in query)
            {
                result.AppendLine($"-Song #{++counter}");
                result.AppendLine($"---SongName: {song.SongName}");
                result.AppendLine($"---Writer: {song.WriterName}");

                if (song.Performers.Count > 0)
                {
                    foreach (var perf in song.Performers)
                    {
                        result.AppendLine($"---Performer: {perf.FullName}");
                    }
                }

                result.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                result.AppendLine($"---Duration: {song.Duration:c}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
