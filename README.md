# POST File Action

This is a github action, which sends the contents of a file to a web api.
This can be done using POST, PUT, or PATCH

## Usage
It's useful for applications, which store (and read) their configuration in a database 
but still want to make use of all the advantages of a proper code integration (configuration as code) like:
- version control
- quality assurance through review
- automatic update upon triggers like open a PR or merge into main
