import "./EventsList.css";
import {Link} from 'react-router-dom';
import React, {useEffect, useState} from "react";
import {IServiceResult} from "../../domain/IServiceResult";
import {IEvent} from "../../domain/IEvent";
import {EventService} from "../../services/EventService";
import toDateOnly from "../../helpers/toDateOnly";
import {ParticipantService} from "../../services/ParticipantService";
import {IParticipant} from "../../domain/IParticipant";

let eventInitialState: IServiceResult<IEvent[]>;
const EventsList = () => {

    const eventService = new EventService();
    const participantService = new ParticipantService();

    const[event, setEvent] = useState(eventInitialState)

    useEffect(() => {
        eventService.getAll().then(data => setEvent(data));
    }, [])

    async function deleteItem(id: string): Promise<void>{
        const getEvent = await eventService.getById(id);
        for (const participant of getEvent.data?.participants!) {
            await participantService.delete(participant.id as string);
        }
        await eventService.delete(id);
        setEvent({
            status: event.status,
            data: event.data?.filter(item => item.id !== id),
            errorMsg: event.errorMsg
        });
    }

    if (!event){
        return null;
    }

    const upcomingEvents = event.data?.filter(item => !item.isHeld);
    const heldEvents = event.data?.filter(item => item.isHeld);

    function calculateParticipants(participants: IParticipant[]): number{
        let res = 0;
        for (const participant of participants) {
            if (participant.enterprise != null){
                res += participant.enterprise.participantsNumber
            }
            if (participant.person != null){
                res += 1;
            }
        }
        return res;
    }

    return (
        <div className="row align-items-md-stretch eventsList">
            <div className="col-md-6">
                <div className="text-white bg-primary rounded-3">
                    <p style={{fontSize:'20px'}} className="d-flex align-items-center justify-content-center h-100">
                        Tulevased üritused
                    </p>
                </div>
                <div className="container bg-white border rounded-3 eventsContainer">
                    {upcomingEvents!.map((item, index) => (
                        <div key={index} className="row eventItem">
                            <div className="col-md-4">
                                {index + 1}.{item.name}
                            </div>
                            <div className="col-md-2">
                                {toDateOnly(item.eventTime)}
                            </div>
                            <div className="col-md-2">
                                {item.location}
                            </div>
                            <div className="col-md-4">
                                <Link
                                    className="participants"
                                    to={`event/${item.id}`}
                                >
                                    Osavõtjad({calculateParticipants(item.participants as IParticipant[])})
                                </Link>
                                <button className="close-button" onClick={() => deleteItem(item.id!)}>✖</button>
                            </div>
                        </div>
                    ))}
                    <div>
                        <Link className="navbar-brand addEvent" to="/addevent">Ürituse lisamine</Link>
                    </div>
                </div>
            </div>
            <div className="col-md-6">
                <div className="text-white bg-primary rounded-3">
                    <p style={{fontSize:'20px'}} className="d-flex align-items-center justify-content-center h-100">
                        Toimunud üritused
                    </p>
                </div>
                <div className="container bg-white border rounded-3 eventsContainer">
                    {heldEvents!.map((item, index) => (
                        <div key={index} className="row eventItem">
                            <div className="col-md-4">
                                {index + 1}.{item.name}
                            </div>
                            <div className="col-md-2">
                                {toDateOnly(item.eventTime)}
                            </div>
                            <div className="col-md-2">
                                {item.location}
                            </div>
                            <div className="col-md-4">
                                <Link className="participants" to={`event/${item.id}`}>Osavõtjad({calculateParticipants(item.participants as IParticipant[])})</Link>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}

export default EventsList;