GraphQL in Dot Net Core.

* Data Loader: 
	N + 1 => We first perform a query then for every item in the list we perform N query.

* Issue - 1: 
	The bearer was not authenticated. Failure message: Firebase ID token has incorrect issuer (iss) claim.
	Expected https://securetoken.google.com/project_id but got https://identitytoolkit.google.com/.
	Make sure the ID token comes from the same Firebase project as the credential used to initialize this SDK.

	Reason: I didn't use the "returnSecureToken": true with email and password in the body while login.

	Solution: Set "returnSecureToken" to true while logging. it will change the issuer (iss) from https://identitytoolkit.google.com/ to https://securetoken.google.com/project_id

