import { twMerge } from "tailwind-merge";

interface BoxProps {
    children : React.ReactNode;
    classname? : string;
}

const Box: React.FC<BoxProps>= ({children, classname}) => {
    return (
        <div
            className={ twMerge(`
                
                bg-dark-amethyst-800
                rounded-lg
                h-fit
                w-full
            `,
                classname
            )}
        >
            {children}
        </div>
    );
}

export default Box;