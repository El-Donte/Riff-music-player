import AccountContent from "./components/AccountContent";

const Account = () => {
    return (
        <div
            className="
                bg-background
                rounded-lg
                h-full
                w-full
                overflow-hidden
                overflow-y-auto
            "
        >
            <AccountContent/>
        </div>
    );
};

export default Account;