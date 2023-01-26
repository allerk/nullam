import libled from "../images/libled.jpg";
import "./BannerSmall.css";

interface IProps{
    pageName: string
}

const BannerSmall = (props: IProps) => {
    return (
        <div className="addEventBanner">
            <div className="row align-items-md-stretch" style={{ backgroundColor: "#0d6efd", margin: 0}}>
                <div className="col-md-3">
                    <div className="text-white bg-primary parent">
                        <p style={{ fontSize:'20px'}} className="center-text">
                            {props.pageName}
                        </p>
                    </div>
                </div>
                <div className="col-md-9 d-flex justify-content-end addEventBannerImage">
                    <img style={{objectFit: "cover", width: "100%", height: "75px"}} src={libled} alt={libled}/>
                </div>
            </div>
        </div>
    )
}

export default BannerSmall;