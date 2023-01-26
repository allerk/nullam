import {EventService} from "../../services/EventService";
import "./AddEventForm.css";
import {Link, useNavigate} from "react-router-dom";
import {FormEvent, useState, useEffect} from "react";
import {IEvent} from "../../domain/IEvent";

interface IProps{
    pageName: string,
    currentEventId?: string
}

const AddEventForm = (props: IProps) => {

    const eventService = new EventService();

    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        id: '',
        name: '',
        eventTime: '',
        location: '',
        isHeld: false,
        additionalDetails: ''
    });

    useEffect(() => {
        if (props.currentEventId){
            eventService.getById(props.currentEventId as string).then(data => setFormData({
                id: data.data?.id as string,
                name: data.data?.name as string,
                eventTime: convertDate(data.data?.eventTime as string) as string,
                location: data.data?.location as string,
                isHeld: data.data?.isHeld as boolean,
                additionalDetails: data.data?.additionalDetails as string
            }));
        }
    }, [])

    const handleChange = (event:
                              EventTarget & HTMLInputElement |
                              EventTarget & HTMLSelectElement |
                              EventTarget & HTMLTextAreaElement) => {
        setFormData({ ...formData, [event.name]: event.value });
    }

    const handleSubmit = async (event: FormEvent, eventId?: string) => {
        event.preventDefault();
        const eventEntity: IEvent = {
            name: formData.name,
            eventTime: formData.eventTime,
            location: formData.location,
            isHeld: false,
            additionalDetails: formData.additionalDetails
        }
        if (eventId){
            eventEntity.id = eventId
            await eventService.put(eventEntity, eventId as string);
        } else {
            await eventService.add(eventEntity)
        }
        navigate("/");
    }

    function convertDate(dateString: string): string {
        // parse the date string using the specified format
        let date = new Date(dateString);
        // convert the date to the desired format
        let formattedDate = date.toISOString().slice(0,16);
        return formattedDate;
    }

    return (
        <div className="container-md" style={{backgroundColor: "white"}}>
            <div className="row align-items-md-stretch">
                <div className="col-md-3"></div>
                <div className="col-md-6">
                    <form onSubmit={(e) => handleSubmit(e, props.currentEventId)}>
                        <h3 style={{color: "#0d6efd"}}>{props.pageName}</h3>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-3">
                                <label htmlFor="name" className="col-form-label">Ürituse nimi:</label>
                            </div>
                            <div className="col-md-9">
                                <input
                                    required
                                    disabled={formData.isHeld}
                                    type="text"
                                    name="name"
                                    value={formData.name}
                                    onChange={(e) => handleChange(e.target)}
                                    className="form-control"
                                />
                            </div>
                        </div>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-3">
                                <label htmlFor="eventTime" className="col-form-label">Toimumisaeg:</label>
                            </div>
                            <div className="col-md-9">
                                <input
                                    required
                                    disabled={formData.isHeld}
                                    type="datetime-local"
                                    name="eventTime"
                                    value={formData.eventTime}
                                    onChange={(e) => handleChange(e.target)}
                                    className="form-control"
                                    min={new Date().toISOString().slice(0,16)}
                                />
                            </div>
                        </div>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-3">
                                <label htmlFor="location" className="col-form-label">Koht:</label>
                            </div>
                            <div className="col-md-9">
                                <input
                                    required
                                    disabled={formData.isHeld}
                                    type="text"
                                    name="location"
                                    value={formData.location}
                                    onChange={(e) => handleChange(e.target)}
                                    className="form-control"
                                />
                            </div>
                        </div>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-3">
                                <label htmlFor="additionalDetails" className="col-form-label">Lisainfo:</label>
                            </div>
                            <div className="col-md-9">
                                <textarea
                                    name="additionalDetails"
                                    disabled={formData.isHeld}
                                    value={formData.additionalDetails}
                                    onChange={(e) => handleChange(e.target)}
                                    className="form-control"
                                    style={{height: "100px"}}
                                    maxLength={1000}
                                ></textarea>
                            </div>
                        </div>
                        <Link to="/" type="button" className="btn btn-secondary buttonsMargin" style={{marginLeft: 0}}>Tagasi</Link>
                        {/*<button type="button" className="btn btn-primary buttonsMargin">Lisa</button>*/}
                        {!formData.isHeld &&
                            <>
                                {props.currentEventId &&
                                    <input type="submit" value="Salvesta" className="btn btn-primary buttonsMargin"/>
                                }
                                {!props.currentEventId &&
                                    <input type="submit" value="Lisa" className="btn btn-primary buttonsMargin"/>
                                }
                            </>
                        }
                    </form>
                </div>
                <div className="col-md-3"></div>
            </div>
        </div>
    )
}

export default AddEventForm;