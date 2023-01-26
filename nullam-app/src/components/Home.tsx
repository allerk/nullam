import './Home.css';
import Banner from "./Banner";
import EventsList from "./event/EventsList";
const Home = () => {
    return (
        <div className="container">
            <Banner></Banner>
            <EventsList></EventsList>
        </div>
    )
};

export default Home;