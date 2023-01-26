import pilt from "../images/pilt.jpg";
import "./Banner.css";

const Banner = () => {
    return (
        <div className="container-md banner">
            <div className="row align-items-md-stretch">
                <div className="col-md-6">
                    <div className="text-white bg-primary parent">
                        <p style={{ fontSize:'20px' }} className="center-text">
                            Proin ullamcorper, libero eget feugiat pellentesque, neque leo semper lectus, quis efficitur velit neque eu dolor. Nunc sem turpis, molestie vitae pretium sit amet, molestie sed justo. In vitae vulputate est. Curabitur vitae tempor augue, ac dapibus est. Donec tempor ante vel finibus dictum.
                        </p>
                    </div>
                </div>
                <div className="col-md-6 d-flex justify-content-end bannerImage">
                    <img style={{objectFit: "cover", width: "100%", height: "319px"}} src={pilt} alt={pilt}/>
                </div>
            </div>
        </div>
    )
}

export default Banner;