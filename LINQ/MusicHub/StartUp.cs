namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));

        }

        //public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)  // => Stoyan (SoftIni) solution 100 points
        //{
        //    var albumInfo = context.Producers.FirstOrDefault(x => x.Id == producerId)
        //        .Albums
        //        .Select(x => new
        //        {
        //            AlbumName = x.Name,
        //            ReleaseDate = x.ReleaseDate,
        //            ProducerName = x.Producer.Name,
        //            Songs = x.Songs.Select(x => new
        //            {
        //                SongName = x.Name,
        //                Price = x.Price,
        //                Writer = x.Writer.Name
        //            })
        //            .OrderByDescending(x => x.SongName)
        //            .ThenBy(x => x.Writer)
        //            .ToList(),
        //            AlbumPrice = x.Price,


        //        })
        //        .OrderByDescending(x => x.AlbumPrice)
        //        .ToList();

        //    StringBuilder sb = new StringBuilder();

        //    foreach (var album in albumInfo)
        //    {
        //        sb.AppendLine($"-AlbumName: {album.AlbumName}");
        //        sb.AppendLine($"-ReleaseDate: {album.ReleaseDate:MM/dd/yyyy}");
        //        sb.AppendLine($"-ProducerName: {album.ProducerName}");
        //        sb.AppendLine($"-Songs:");

        //        int counter = 0;

        //        foreach (var song in album.Songs)
        //        {

        //            sb.AppendLine($"---#{++counter}");
        //            sb.AppendLine($"---SongName: {song.SongName}");
        //            sb.AppendLine($"---Price: {song.Price:f2}");
        //            sb.AppendLine($"---Writer: {song.Writer}");
        //        }

        //        sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
        //    }

        //    return sb.ToString().TrimEnd();

        //}

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)  // => My solution 100 points
        {
            var albums = context.Albums.Where(x => x.ProducerId == producerId)
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate,
                    ProducerName = x.Producer.Name,
                    AlbumPrice = x.Price,
                    Songs = x.Songs.Select(x => new { x.Name, x.Price, WriterName = x.Writer.Name })
                                    .OrderByDescending(x => x.Name)
                                    .ThenBy(x => x.WriterName).ToList()

                })
                .ToList()
                .OrderByDescending(x => x.AlbumPrice);

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");
                int counter = 0;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{++counter}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.WriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {

            var songs = context.Songs.ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new
                {
                    SongName = x.Name,
                    Writer = x.Writer.Name,
                    Performer = x.SongPerformers
                            .Select(x => x.Performer.FirstName + " " + x.Performer.LastName)
                            .FirstOrDefault(),
                    AlbumProducer = x.Album.Producer.Name,
                    SongDuration = x.Duration
                })
                .ToList()
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.Writer)
                .ThenBy(x => x.Performer);


            StringBuilder sb = new StringBuilder();
            int counter = 0;

            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{++counter}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.Writer}");
                sb.AppendLine($"---Performer: {song.Performer}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.SongDuration:c}");
            }

            return sb.ToString().TrimEnd();

        }
    }
}
