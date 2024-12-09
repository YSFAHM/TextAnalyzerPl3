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