"use client";

import * as RadixSlider from "@radix-ui/react-slider"
import { twMerge } from "tailwind-merge";

interface SliderProps {
    value?: number;
    max: number;
    step: number;
    className: string;
    onChange?: (value: number) => void;
}

const Slider: React.FC<SliderProps> = ({value = 1, onChange, max = 1, step = 0.1, className = ""}) => {
    const handleChange = (newValue: number[]) => {
        onChange?.(newValue[0]);
    }
    return (
        <RadixSlider.Root
            className={twMerge(`
                relative
                flex
                items-center
                select-none
                touch-none
                w-full
                h-10
            `, className)}
            defaultValue={[1]}
            value={[value]}
            onValueChange={handleChange}
            max={max}
            step={step}
            aria-label="Volume"
        >
            <RadixSlider.Track
                className="
                    bg-neutral-500
                    relative
                    grow
                    rounded-full
                    h-[3px]
                "
            >
                <RadixSlider.Range
                    className="
                        absolute
                        bg-white
                        rounded-full
                        h-full
                    "
                />
            </RadixSlider.Track>
        </RadixSlider.Root>
    );
};

export default Slider;