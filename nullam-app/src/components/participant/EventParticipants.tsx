import {Link, useNavigate, useParams} from "react-router-dom";
import {EventService} from "../../services/EventService";
import React, {FormEvent, useEffect, useState} from "react";
import {IServiceResult} from "../../domain/IServiceResult";
import {IEvent} from "../../domain/IEvent";
import BannerSmall from "../BannerSmall";

import "../event/AddEventForm.css";
import toDateOnly from "../../helpers/toDateOnly";
import {ParticipantService} from "../../services/ParticipantService";
import "./EventParticipants.css";
import AddParticipantToEvent from "./AddParticipantToEvent";
import {IParticipant} from "../../domain/IParticipant";
import {PersonService} from "../../services/PersonService";
import {EnterpriseService} from "../../services/EnterpriseService";

let eventInitialState: IServiceResult<IEvent>;
const EventParticipants = () => {

    const eventService = new EventService();
    const participantService = new ParticipantService();
    const personService = new PersonService();
    const enterpriseService = new EnterpriseService();

    const { eventId } = useParams();

    const [eventState, setEventState] = useState(eventInitialState)

    useEffect(() => {
        eventService.getById(eventId as string).then(data => setEventState(data));
    }, [])

    if (!eventState){
        return null;
    }

    const deleteParticipant = async (id: string) => {
        const eventCopy = eventState.data;
        eventCopy!.participants = eventState.data?.participants?.filter(item => item.id !== id);
        await participantService.delete(id);

        setEventState({
            status: eventState.status,
            data: eventCopy,
            errorMsg: eventState.errorMsg
        });
    }

    const attachParticipantToEvent = async (participant: IParticipant) => {
        const eventCopy = eventState.data;
        eventCopy?.participants?.push(participant)
        setEventState({
            status: eventState.status,
            data: eventCopy,
            errorMsg: eventState.errorMsg
        })
    }

    return (
        <div className="container-md">
            <BannerSmall pageName={"Osavõtjad"}></BannerSmall>
            <div className="container-md" style={{backgroundColor: "white"}}>
                <div className="row align-items-md-stretch">
                    <div className="col-md-3"></div>
                    <div className="col-md-7">
                        <h3 style={{color: "#0d6efd"}}>Osavõtjad</h3>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-2 details">
                                Ürituse nimi:
                            </div>
                            <div className="col-md-10">
                                <div className="container-md">
                                    {eventState.data?.name}
                                </div>
                            </div>
                        </div>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-2 details">
                                Toimumisaeg:
                            </div>
                            <div className="col-md-10">
                                <div className="container-md">
                                    {toDateOnly(eventState.data?.eventTime as string)}
                                </div>
                            </div>
                        </div>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-2 details">
                                Koht:
                            </div>
                            <div className="col-md-10">
                                <div className="container-md">
                                    {eventState.data?.location}
                                </div>
                            </div>
                        </div>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-2 details">
                                Osavõtjad:
                            </div>
                            <div className="col-md-10">
                                {eventState.data?.participants?.map((item, index) => (
                                    <div key={index} className="container-md">
                                        {item.person !== null ? (
                                            <div key={index} className="row align-items-md-stretch">
                                                <div className="col-md-4">
                                                    {index + 1}. {item.person!.firstName + " " + item.person!.lastName}
                                                </div>
                                                <div className="col-md-3">
                                                    {item.person!.identificationNumber}
                                                </div>
                                                <div className="col-md-3">
                                                    <Link
                                                        className="participants"
                                                        to={`/participant/${item.id}`}
                                                        state={{ participantType: "0", eventId: `${item.eventId}`, identificationNumber: `${item.person?.identificationNumber}`, isHeld: `${eventState.data?.isHeld}` }}
                                                    >
                                                        Vaata
                                                    </Link>
                                                </div>
                                                <div className="col-md-2">
                                                    {!eventState.data?.isHeld &&
                                                        <button className="text-like-button participants" onClick={() => deleteParticipant(item.id!)}>Kustuta</button>
                                                    }
                                                </div>
                                            </div>
                                        ) : (
                                            <div key={index} className="row align-items-md-stretch">
                                                <div className="col-md-4">
                                                    <div>
                                                        {index + 1}. {item.enterprise!.legalName}
                                                    </div>
                                                    <sub>(osavõtjate arv: {item.enterprise?.participantsNumber})</sub>
                                                </div>
                                                <div className="col-md-3">
                                                    {item.enterprise!.registerCode}
                                                </div>
                                                <div className="col-md-3">
                                                    <Link
                                                        className="participants"
                                                        to={`/participant/${item.id}`}
                                                        state={{ participantType: "1", eventId: `${item.eventId}`, registerCode: `${item.enterprise?.registerCode}`, isHeld: `${eventState.data?.isHeld}` }}
                                                    >
                                                        Vaata
                                                    </Link>
                                                </div>
                                                <div className="col-md-2">
                                                    {!eventState.data?.isHeld &&
                                                        <button className="text-like-button participants" onClick={() => deleteParticipant(item.id!)}>Kustuta</button>
                                                    }
                                                </div>
                                            </div>
                                        )}
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                    <div className="col-md-2"></div>
                    <AddParticipantToEvent eventId={eventId as string} attachParticipantToEvent={attachParticipantToEvent} isHeld={eventState.data!.isHeld}></AddParticipantToEvent>
                </div>
            </div>
        </div>
    )
}

export default EventParticipants;