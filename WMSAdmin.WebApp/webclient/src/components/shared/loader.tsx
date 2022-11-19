import { useTrackedGlobalState } from '../../utilities/store';
import { UIHelper } from '../../utilities/uihelper';
import logo from '../../logo.svg';
import { AppData, AppState } from '../../entities/configs';

export const Loader = () => {
    let appConfig:AppState = useTrackedGlobalState();
    let loaderClass = " modal-show ";

    if (appConfig !== undefined) {
        loaderClass = UIHelper.getModalClass(appConfig.showLoader);
    }
    return (
        <div className={"modal" + loaderClass} tabIndex={-1} role="dialog">
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content bg-transparent border-0">
                    <div className="modal-body d-inline-flex justify-content-center p-0 m-0 bg-transparent">
                        <img src={logo} className="spinner-border spinner-grow border-warning bg-white" style={{ height: '4rem', width: '4rem', borderRadius: "60%" }} alt="logo" />
                    </div>
                </div>
            </div>
        </div>
    );
};

export const LoaderStart = () => {
    return (
        <div className="modal modal-show" tabIndex={-1} role="dialog">
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content bg-transparent border-0">
                    <div className="modal-body d-inline-flex justify-content-center p-0 m-0 bg-transparent">
                        <img src={logo} className="spinner-border spinner-grow border-warning bg-white" style={{ height: '4rem', width: '4rem', borderRadius: "60%" }} alt="logo" />
                    </div>
                </div>
            </div>
        </div>
    );
};