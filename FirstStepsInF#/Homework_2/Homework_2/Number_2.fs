module Number_2

type 't BinaryTree = 
    | Node of 't * 't BinaryTree * 't BinaryTree
    | Leaf

let rec map f = function
    | Node (data, leftTree, rightTree) -> Node(f data, map f leftTree, map f rightTree)
    | Leaf -> Leaf