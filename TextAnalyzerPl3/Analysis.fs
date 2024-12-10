module Analysis

open System

let countWords (text: string) = 
    text.Split([|' '; '\n'; '\t'|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.length

let countSentences (text: string) = 
    text.Split([|'.'; '!'; '?'|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.length

let countParagraphs (text: string) = 
    text.Split([|'\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.length

let wordFrequency (text: string) = 
    text.Split([|' '; '\n'; '\t'; '.'; ','; '!'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun word -> word.ToLower())
    |> Array.countBy id
    |> Array.sortByDescending snd

let averageSentenceLength text =
    let totalWords = countWords text
    let totalSentences = countSentences text
    if totalSentences > 0 then float totalWords / float totalSentences else 0.0