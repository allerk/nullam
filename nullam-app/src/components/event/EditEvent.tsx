import {EventService} from "../../services/EventService";
import AddEventForm from "./AddEventForm";
import { useParams } from "react-router-dom";
import BannerSmall from "../BannerSmall";

const EditEvent = () => {

    const { eventId } = useParams();

    return (
        <div className="container-md">
            <BannerSmall pageName={"Ürituse muutmine"}></BannerSmall>
            <AddEventForm currentEventId={eventId} pageName={"Ürituse info"}></AddEventForm>
        </div>
    )
}

export default EditEvent;