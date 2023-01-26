import React from 'react';
import './App.css';
import Header from "./components/Header";
import Footer from "./components/Footer";
import {Route, Routes} from "react-router-dom";
import Home from "./components/Home";
import AddEvent from "./components/event/AddEvent";

import "./App.css";
import EventParticipants from "./components/participant/EventParticipants";
import ParticipantDetails from "./components/participant/ParticipantDetails";
import EditEvent from "./components/event/EditEvent";

function App() {
  return (
      <div>
          <div className="container">
              <Header/>
              <main role="main" className="pb-3">
                  <Routes>
                      <Route index element={<Home />}/>
                      <Route path="/addevent" element={<AddEvent />}></Route>
                      <Route path="/event/:eventId" element={<EventParticipants></EventParticipants>}></Route>
                      <Route path="/eventEdit/:eventId" element={<EditEvent></EditEvent>}></Route>
                      <Route path="/participant/:participantId" element={<ParticipantDetails></ParticipantDetails>}></Route>
                  </Routes>
              </main>
              <Footer/>
          </div>
      </div>
  );
}

export default App;
