 
## TermbinSharp
termbinSharp is a high-performance server application that mimics the functionality of termbin.com. It allows you to host your own private pastebin service for sharing text snippets securely and efficiently. With built-in caching, it provides fast and reliable text sharing.

## Features
* Self-Hosted: Run your own pastebin server and have full control over your data and content.

* Caching: termbinSharp caches uploaded text snippets, reducing server load and improving response times.

* Secure: Designed with security in mind, ensuring the privacy and integrity of shared text.
  
* No Database: Everything is saved and read via files.

## Using the TermbinSharp Server
TermbinSharp provides a straightforward API for uploading and retrieving text snippets. You can interact with the server using HTTP requests. Here's a guide on how to use the TermbinSharp server:

### Uploading Text Snippets
To upload a text snippet to the TermbinSharp server, you can make a POST request to the /data/set endpoint. The text snippet should be included in the request body as a JSON string. Here's an example using curl:

```
curl -X POST -H "Content-Type: application/json" -d "This is my text snippet" http://your-server-address/data/set
```
Replace http://your-server-address with the actual address where your TermbinSharp server is running. Upon successful upload, the server will respond with a 200 OK status with the corresponding direct url to retrive the data later.

Retrieving Text Snippets
To retrieve a previously uploaded text snippet, you can make a GET request to the / endpoint followed by the snippet's unique identifier (path). For example:

```
curl http://your-server-address/abc123
```
Replace abc123 with the actual unique identifier of the text snippet you want to retrieve. If the snippet exists, the server will respond with a 200 OK status and the content of the text snippet in the response body.
