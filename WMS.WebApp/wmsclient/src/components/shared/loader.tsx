import { useTrackedGlobalState } from '../../utilities/store';
import { UIHelper } from '../../utilities/uihelper';
import logo from '../../logo.svg';

export const Loader = () => {
    const appConfig = useTrackedGlobalState();
    return (
        <div className={"modal" + UIHelper.getModalClass(appConfig.showLoader)} tabIndex={-1} role="dialog">
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