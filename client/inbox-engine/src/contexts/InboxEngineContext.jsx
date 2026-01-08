import React, { createContext, useState, useContext } from "react";
import payload from "../data/testData";

const InboxEngineContext = createContext();

// Context class for the 
export const InboxEngineProvider = ({ children }) => {

    const [emails, setEmails] = useState([]);
    const [error, setError] = useState(null);

    // Fetch the emails from the InboxEngine API.
    const handleFetchEmailsClick = () => {
        // Refresh email and error status
        setEmails([]);
        setError(null);

        const emailServiceUrl = 'http://localhost:5000/api/inbox/sort';

        const options = {
          method: "POST",
          headers: new Headers({'Content-Type': 'application/json'}),
          body: JSON.stringify(payload),
        };

        // Successful fetch case - set the email list
        fetch(emailServiceUrl, options)
            .then(response => response.json())
            .then(emails => { 
                setEmails(emails)
        }).catch(e => { // Unsuccessful fetch case - set the error
            setError("An error occurred while getting list of emails");
            console.error(e);
        });

    }    

    // Return the emails, the methods to fetch the emails from the emails API, and the error object
    return (
        <InboxEngineContext value={{emails, 
                handleFetchEmailsClick,
                error }}>
            {children}
        </InboxEngineContext>
    )
}

export const useInboxEngineContext = () => {
    const ctx = useContext(InboxEngineContext);
    if(!ctx) throw new Error("Context not found");
    return ctx;
}