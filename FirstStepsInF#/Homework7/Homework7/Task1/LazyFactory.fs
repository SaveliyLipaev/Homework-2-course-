module Homework7.Task1.LazyFactory

open Homework7.Task1.ILazy
open Homework7.Task1.LazyImplementations

// Represents factory which produces different types of lazy
// for single-threaded mode, multi-threaded mode; and lock-free for multi-threaded mode
type LazyFactory() =
    static member CreateSingleThreadedLazy<'a>(supplier: unit -> 'a) =
        new SingleThreadedLazy<'a>(supplier) :> ILazy<'a>

    static member CreateMultiThreadedLazy<'a>(supplier: unit -> 'a) =
        MultiThreadedLazy<'a>(supplier) :> ILazy<'a>

    static member CreateLockFreeLazy<'a when 'a: not struct and 'a: equality>(supplier: unit -> 'a) =
        LockFreeLazy<'a>(supplier) :> ILazy<'a>