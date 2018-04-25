# Concordance

Generates the concordance of a given input text file or string.

## Prerequisites

* dotnet core >= 2.0

## Assumptions

* Sentence terminator is defined as one of the following: . ! ?
* Sentences are defined as a string of letters or characters until either of the following cases are satisfied:
  * a sentence terminator followed by some whitespace followed by an uppercase letter, or
  * the end of the file
* Words with differing punctuation or case, but the same letters in the same order are treated the same, as in "i.e." compared to "ie" or "Given" compared to "given".

## Install

```
git clone https://github.com/holotrek/concordance.git
dotnet restore
```

## Run

```
cd Concordance
dotnet run ../test.txt
```

## Test

```
cd Concordance.Tests
dotnet test
```
