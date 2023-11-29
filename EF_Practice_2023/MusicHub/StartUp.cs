namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //int producerId = int.Parse(Console.ReadLine());
            //string task1 = ExportAlbumsInfo(context, producerId);

            int length = int.Parse(Console.ReadLine());
            string task2 = ExportSongsAboveDuration(context, length);

            Console.WriteLine(task2);

        }

        public static string ExportAlbumsInfo(MusicHubDbContext db, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var query = db.Producers
                .FirstOrDefault(x => x.Id == producerId)
                   .Albums.Select(x => new
                   {
                       AlbumName = x.Name,
                       ProducerName = x.Producer.Name,
                       ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                       AlbumPrice = x.Price,
                       AlbumSongs = x.Songs
                       .Select(x => new
                       {
                           SongName = x.Name,
                           SongPrice = x.Price,
                           Writer = x.Writer.Name
                       })
                   }).ToList();


            foreach (var album in query.OrderByDescending(x => x.AlbumPrice))
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}")
                .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                .AppendLine($"-ProducerName: {album.ProducerName}")
                .AppendLine($"-Songs:");
                int couter = 1;

                foreach (var song in album.AlbumSongs.OrderByDescending(x => x.SongName).ThenBy(x => x.Writer))
                {
                    sb.AppendLine($"---#{couter}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Price: {song.SongPrice:f2}")
                    .AppendLine($"---Writer: {song.Writer}");
                    couter++;
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext db, int duration)
        {
            var query = db.Songs.Where(x => (x.Duration.Minutes * 60 + x.Duration.Seconds) > duration)
                .Select(x => new
                {
                    SongName = x.Name,
                    WriterName = x.Writer.Name,
                    SongDuration = x.Duration,
                    AlbumProducer = x.Album.Producer.Name,
                    Performers = x.SongPerformers.Select(x => new
                    {
                        FullName = x.Performer.FirstName + " " + x.Performer.LastName
                    }),
                }).ToList();

            int counter = 1;
            StringBuilder sb = new StringBuilder();

            foreach (var song in query.OrderBy(x => x.SongName).ThenBy(x => x.WriterName))
            {
                sb.AppendLine($"-Song #{counter++}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WriterName}");

                if (song.Performers.Count() > 0)
                {
                    foreach (var performes in song.Performers.OrderBy(x => x.FullName))
                    {
                        sb.AppendLine($"---Performer: {performes.FullName}");
                    }
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.SongDuration:c}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
