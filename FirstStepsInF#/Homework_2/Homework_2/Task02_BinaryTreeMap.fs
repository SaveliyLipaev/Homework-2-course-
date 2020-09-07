module BinaryTreeMap

type Tree<'t> =
    | Node of value: 't * left: Tree<'t> * right: Tree<'t>
    | Empty


(* Рекурсивная функция, применяет фунцию f() к каждому элементу дерева
   возвращающая копию бинарного дерева *)
let rec map tree f =
    match tree with
    | Node (value, left, right) -> Node(f value, map left f, map right f)
    | Empty -> Empty