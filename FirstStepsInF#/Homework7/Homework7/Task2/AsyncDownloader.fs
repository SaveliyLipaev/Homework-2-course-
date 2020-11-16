module Homework7.Task2.AsyncDownloader

open System.IO
open System.Net
open System.Text.RegularExpressions

/// Describes the final download status
type DownloadStatus<'a> =
    | Success of 'a // 'a as downloaded data
    | Error of string

/// Returns the size of the pages that the page with url refers to, including the page itself
let downloadAsync (url: string) =
    // regex used in URL search
    let urlRegex =
        Regex("<a href\s*=\s*\"(https?://[^\"]+)\"\s*>", RegexOptions.Compiled)

    /// Loads the page by URL asynchronously
    let downloadPageAsync (_url: string) =
        async {
            try
                let request = WebRequest.Create(_url)
                use! response = request.AsyncGetResponse()
                use stream = response.GetResponseStream()
                use reader = new StreamReader(stream)
                let html = reader.ReadToEnd()
                return (_url, Success(html))
            with error -> return (_url, Error(error.Message))
        }

    let mainPageResult =
        downloadPageAsync (url) |> Async.RunSynchronously

    match mainPageResult with
    | (_, Success (htmlDocument)) ->
        // load all pages referenced by main
        let matches = urlRegex.Matches(htmlDocument)

        let tasks =
            [ for _match in matches -> downloadPageAsync (_match.Groups.[1].Value) ]

        let results =
            Async.Parallel tasks |> Async.RunSynchronously

        mainPageResult :: (results |> Array.toList)

    | _ -> [ mainPageResult ]

/// Prints result
let printDownloaded (url) =
    let results = downloadAsync (url)
    for result in results do
        match result with
        | (_url, Success (x)) -> printfn "[%s - %d symbols]" _url x.Length
        | (_url, Error (x)) -> printfn "[%s - error: %s]" _url x
