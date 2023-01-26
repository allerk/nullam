import AddEventForm from "./AddEventForm";
import BannerSmall from "../BannerSmall";

const AddEvent = () => {
    return (
        <div className="container-md">
            <BannerSmall pageName={"Ürituse lisamine"}></BannerSmall>
            <AddEventForm pageName={"Ürituse lisamine"}></AddEventForm>
        </div>

    )
}

export default AddEvent;