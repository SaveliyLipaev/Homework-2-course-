namespace Task_2

module ThreeDigitPalindrome  = 
        let isStringPalindrome (s: int) =
            let arr = s.ToString().ToCharArray()
            arr = Array.rev arr
    
        let threeDigitNumbersList = [100 .. 999]
        
        let getMaxPalindrome () = 
            threeDigitNumbersList
            |> List.collect (fun number -> List.map ((*) number) threeDigitNumbersList) 
            |> List.filter isStringPalindrome
            |> List.max 
            |> string
