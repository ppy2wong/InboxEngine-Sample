import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { InboxEngineProvider } from './contexts/InboxEngineContext.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <InboxEngineProvider>
      <App />
    </InboxEngineProvider>
  </StrictMode>,
)
