import './App.css'
import { useInboxEngineContext } from "./contexts/InboxEngineContext";
import { useState } from 'react';

// React-bootstrap element imports
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Stack from 'react-bootstrap/Stack';
import Form from 'react-bootstrap/Form';


function App() {

  // Use a context for methods for fetching emails from InboxEngine API
  const {emails, handleFetchEmailsClick, error} = useInboxEngineContext();
  const [dateTime, setDateTime] = useState("");
  const [todaysDateClickedCount, setTodaysDateClickedCount] = useState(0);

  // Colour the borders and header depending on priority:
  const getPriorityBorder = (priorityScore) => {
    if(priorityScore < 30) { // highlight emails with priority score smaller than 30 in green
      return "success";
    } else if (priorityScore > 70) { // highlight emails with priority score greater than 70 in red
      return "danger";
    } else {
      return "light";
    }
  }

  const setDate = (e) => {
    const dateValue = e.target.value;
    setDateTime(dateValue);
  }

  const changeDate = () => {
      const setDateUrl = 'http://localhost:5000/api/inbox/setTodaysDate';

    const options = {
      method: "POST",
      headers: new Headers({'Content-Type': 'application/json'}),
      body: JSON.stringify(dateTime.toString() + ":00Z"),
    };

    console.log(options);

    fetch(setDateUrl, options)
        .then(response => response.text())
        .then(response => { 
          console.log(response);
          setTodaysDateClickedCount(todaysDateClickedCount + 1);
        }).catch(e => {
          console.error("Cannot set date");
          console.error(e);
        });

  }

  return (
    <Container className="inbox-engine-main-container">
      <Stack gap={3}>
      <h1 className="hero">Inbox Checker</h1>
      <Form>
        <Form.Group controlId="exampleForm.DateControl">
          <Stack direction="horizontal">
          <Form.Label>Today's Date:</Form.Label>
          <Form.Control type="datetime-local" 
            min="2023-10-26T06:00"
            onChange={setDate}
            xs={5}/>
          <Button onClick={changeDate}>Set today's date</Button>
          </Stack>
        </Form.Group>
      </Form>
      <Button onClick={handleFetchEmailsClick} disabled={todaysDateClickedCount === 0}>Load emails</Button>
        {/* Format each email as a card */
        emails !== null && emails.length > 0 && emails.map(
              (num, idx, emailArray) =>
            <Card border={getPriorityBorder(emailArray[idx].priorityScore)}
              key={idx}>
              <Card.Header className={getPriorityBorder(emailArray[idx].priorityScore)}>{emailArray[idx].subject}</Card.Header>
              <Card.Body>
                <Card.Title>Sender: {emailArray[idx].sender}</Card.Title>
                <Card.Subtitle>Priority Score: {emailArray[idx].priorityScore}</Card.Subtitle>
                <Card.Body>{emailArray[idx].body}</Card.Body>
              </Card.Body>
              <Card.Footer>
                Received At: {emailArray[idx].receivedAt} <br />
                Is VIP: {emailArray[idx].isVIP ? "Yes": "No"}  
                </Card.Footer>
            </Card>
            )
        }
        {/* Error message */
        error !== null && (
          <div>
            {error}
          </div>
        )}
      </Stack>

    </Container>
  )
}

export default App;
