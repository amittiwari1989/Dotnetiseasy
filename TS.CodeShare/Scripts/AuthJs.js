var settings = {
    client_id: "1025588121266-9idsjrokgmibit6pf9ci1ojmj729i8kf.apps.googleusercontent.com",
    callback_url: "localhost:58516/Account/Register"
};

var GoogleAuth; // Google Auth object.
function initClient() {
    gapi.client.init({
        'apiKey': 'SxGto8rvKU2kYHVEdBqLPRGE',
        'clientId': settings.client_id,
        'scope': 'https://www.googleapis.com/auth/drive.metadata.readonly',
        'discoveryDocs': ['https://www.googleapis.com/discovery/v1/apis/drive/v3/rest']
    }).then(function () {
        GoogleAuth = gapi.auth2.getAuthInstance();

        // Listen for sign-in state changes.
        GoogleAuth.isSignedIn.listen(updateSigninStatus);
    });
}

function SignIn() {

    GoogleAuth.signIn();
}