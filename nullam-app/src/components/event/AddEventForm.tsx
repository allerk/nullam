import {EventService} from "../../services/EventService";
import "./AddEventForm.css";
import {Link, useNavigate} from "react-router-dom";
import {FormEvent, useState} from "react";
import {IEvent} from "../../domain/IEvent";

const AddEventForm = () => {

    const eventService = new EventService();

    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        name: '',
        eventTime: '',
        location: '',
        additionalDetails: ''
    });

    const handleChange = (event:
                              EventTarget & HTMLInputElement |
                              EventTarget & HTMLSelectElement |
                              EventTarget & HTMLTextAreaElement) => {
        setFormData({ ...formData, [event.name]: event.value });
    }

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        const eventEntity: IEvent = {
            name: formData.name,
            eventTime: formData.eventTime,
            location: formData.location,
            isHeld: false,
            additionalDetails: formData.additionalDetails
        }
        await eventService.add(eventEntity)
        navigate("/");
    }

    return (
        <div className="container-md" style={{backgroundColor: "white"}}>
            <div className="row align-items-md-stretch">
                <div className="col-md-3"></div>
                <div className="col-md-6">
                    <form onSubmit={(e) => handleSubmit(e)}>
                        <h3 style={{color: "#0d6efd"}}>Ürituse lisamine</h3>
                        <div className="row align-items-md-stretch formRowItem">
                            <div className="col-md-3">
                                <label htmlFor="name" className="col-form-label">Ürituse nimi:</label>
                            </div>
                            <div className="col-md-9">
                                <input
                                    required
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
                                    type="datetime-local"
                                    name="eventTime"
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
                        <input type="submit" value="Lisa" className="btn btn-primary buttonsMargin"/>
                    </form>
                </div>
                <div className="col-md-3"></div>
            </div>
        </div>
    )
}

export default AddEventForm;