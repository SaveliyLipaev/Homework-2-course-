module Homework4.Phonebook

open System
open System.IO
open System.Runtime.Serialization.Formatters.Binary

// Условие задачи:
// Написать программу - телефонный справочник.
// Она должна уметь хранить имена и номера телефонов,
// в интерактивном режиме осуществлять следующие операции:
//    1. выйти
//    2. добавить запись (имя и телефон)
//    3. найти телефон по имени
//    4. найти имя по телефону
//    5. вывести всё текущее содержимое базы
//    6. сохранить текущие данные в файл
//    7. считать данные из файла

/// Adds new contact to DB
let addToDB phone name listAsDB = (phone, name) :: listAsDB

/// Finds by phone number
let findNameByPhone phone listAsDB =
    listAsDB
    |> List.filter (fun (current, _) -> current = phone)
    |> List.map (snd)

/// Finds by contact name
let findPhoneByName name listAsDB =
    listAsDB
    |> List.filter (fun (_, current) -> current = name)
    |> List.map (fst)

/// Prints data from given DB (list plays DB role)
let rec printData listAsDB =
    match listAsDB with
    | [] -> printfn "%s" "_"
    | h :: tail ->
        printfn "%A" h
        printData tail

/// Saves DB to a DAT file
let saveToFile listAsDB =
    let formatter = BinaryFormatter()

    use stream =
        new FileStream("DB.dat", FileMode.Create)

    formatter.Serialize(stream, box listAsDB)

/// Reads DB from a DAT file by given filename
let readFromFile filename =
    let formatter = BinaryFormatter()
    try
        use stream = new FileStream(filename, FileMode.Open)
        let dat = formatter.Deserialize(stream)
        match dat with
        | :? ((string * string) list) as l -> l
        | _ -> raise (FormatException("Could not parse data; incorrect format"))

    with
    | :? FormatException ->
        raise (Exception("Error error occurred during file read; unexpected format or wrong filename"))
    | :? FileNotFoundException -> raise (ArgumentException(sprintf "Could not find file with this name: %s" filename))

/// Prints options to console
let printInfo =
    printfn "Choose what to do (type operation number):\n"
    printfn "\t1) Exit"
    printfn "\t2) Add new contact"
    printfn "\t3) Find name by phone"
    printfn "\t4) Find phone by name"
    printfn "\t5) Print all"
    printfn "\t6) Save to file"
    printfn "\t7) Read from file"

/// Activates interactive mode
let rec interactiveInterface listAsDB =
    printInfo
    let command = Console.ReadLine()
    match command with
    | "1" -> ignore
    | "2" ->
        printfn "%s" "type phone & name"
        let phone = Console.ReadLine()
        let name = Console.ReadLine()
        printfn "%s" "adding.."
        interactiveInterface (addToDB phone name listAsDB)
    | "3" ->
        printfn "%s" "type phone"
        let phone = Console.ReadLine()
        printfn "%A" (findNameByPhone phone listAsDB)
        interactiveInterface listAsDB
    | "4" ->
        printfn "%s" "type name"
        let name = Console.ReadLine()
        printfn "%A" (findPhoneByName name listAsDB)
        interactiveInterface listAsDB
    | "5" ->
        printData listAsDB
        interactiveInterface listAsDB
    | "6" ->
        saveToFile listAsDB
        printfn "%s" "saving..."
        interactiveInterface listAsDB
    | "7" ->
        printfn "%s" "type file name to load"
        let filename = Console.ReadLine()
        try
            let fileData = readFromFile filename
            printfn "%s" "successful"
            interactiveInterface fileData
        with :? ArgumentException as e ->
            printfn "%s" e.Message
            interactiveInterface listAsDB
    | _ ->
        printfn "%s" "Incorrect command try again"
        interactiveInterface listAsDB

[<EntryPoint>]
let main _ =
    let testDB =
        [ ("8800", "Ilya")
          ("8800", "Dima")
          ("5535", "Mom") ]

    interactiveInterface testDB |> ignore
    0