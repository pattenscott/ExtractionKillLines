You have a CSV with many repeating rows that you want removed.
You create a KillLine.txt and populate it with the row(s) you want removed from the CSV.
The String(s) in the Killline will be INSTR() matched, therefore:
your Killilne could say HELLO
it will remove a line that says 12345HELLO67890
