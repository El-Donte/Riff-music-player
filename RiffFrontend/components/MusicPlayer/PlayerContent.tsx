'use client';

import usePlayer from "@/hooks/usePlayer";
import useSound from "use-sound";
import MediaItem from "../Items/MediaItem";
import LikeButton from "../Basic/LikeButton";
import Slider from '@mui/material/Slider';

import { useEffect, useRef, useState } from "react";
import { BsPauseFill, BsPlayFill } from "react-icons/bs";
import { AiFillStepBackward, AiFillStepForward } from "react-icons/ai";
import { HiSpeakerWave, HiSpeakerXMark } from "react-icons/hi2";
import { TbRepeat, TbRepeatOnce, TbArrowsShuffle } from "react-icons/tb";
import { useMediaSession } from "@/hooks/useMediaSession";

interface PlayerContentProps {
  track: any;
  trackUrl: string;
}

const PlayerContent: React.FC<PlayerContentProps> = ({ track, trackUrl }) => {
  const player = usePlayer();
  const volume = player.volume;
  const [seekValue, setSeekValue] = useState(0);
  const [duration, setDuration] = useState(0);
  const [lastVolume, setLastVolume] = useState(1);

  const repeatModeRef = useRef(player.repeatMode);
  const shuffleModeRef = useRef(player.isShuffled);

  const Icon = player.isPlaying ? BsPauseFill : BsPlayFill;
  const VolumeIcon = volume === 0 ? HiSpeakerXMark : HiSpeakerWave;

  useEffect(() => {
    repeatModeRef.current = player.repeatMode;
    shuffleModeRef.current = player.isShuffled;
  }, [player.repeatMode]);

  useEffect(() => {
    localStorage.setItem("player-volume", volume.toString());
  }, [volume]);

  const [play, { pause, sound }] = useSound(trackUrl, {
    volume: player.volume,
    format: ["mp3", "aac", "ogg", "wav"],
    onplay: () => player.play(),
    onpause: () => player.pause(),
    onend: () => {
      player.pause();
      
      if (repeatModeRef.current !== 'one') {
        player.next();
      }
    }
  });

  useMediaSession(track, play, pause);

  useEffect(() => {
    if (sound) {
      sound.loop(player.repeatMode === 'one');
    }
  }, [player.repeatMode, sound]);

  useEffect(() => {
    sound?.play();
    
    if(player.repeatMode === "one"){
      player.repeatMode = "all";
    }

    return () => {
      sound?.unload();
    };
  }, [sound]);

  const handlePlayPause = () => {
    player.isPlaying ? pause() : play();
  };

  useEffect(() => {
    const interval = setInterval(() => {
      if (sound) {
        setSeekValue(sound.seek() || 0);
        setDuration(sound.duration() || 0);
      }
    }, 500);
    return () => clearInterval(interval);
  }, [sound]);

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60);
    const secs = Math.floor(seconds % 60);
    return `${mins}:${secs.toString().padStart(2, "0")}`;
  };

  return (
  <div className="flex flex-col md:grid md:grid-cols-3 h-full gap-y-4 md:gap-y-0">
    <div className="flex items-center justify-between md:col-span-1">
      <div className="flex items-center gap-x-4">
        <MediaItem track={track} />
        <LikeButton trackId={track.id} />
      </div>
{/*       
      <div className="md:hidden flex items-center gap-x-2">
        <VolumeIcon
          onClick={() => {
            if (volume > 0) {
              setLastVolume(volume);
              player.setVolume(0);
            } else {
              const newVol = lastVolume > 0 ? lastVolume : 1;
              player.setVolume(newVol);
            }
          }}
          size={34}
          className="cursor-pointer"
        />
      </div> */}
    </div>

    <div className="flex flex-col items-center justify-center gap-y-1 md:col-span-1 md:col-start-2">
      <div className="flex items-center gap-x-6">
        <button
            onClick={player.toggleShuffle}
            className={`transition ${player.isShuffled ? "text-purple-500" : "text-neutral-400 hover:text-white"}`}
          >
            <TbArrowsShuffle size={22} />
          </button>

          <AiFillStepBackward
            onClick={player.previous}
            size={30}
            className="text-neutral-400 cursor-pointer hover:text-white transition"
          />

          <div
            onClick={handlePlayPause}
            className="h-10 w-10 flex items-center justify-center rounded-full bg-white p-1 cursor-pointer"
          >
            <Icon size={30} className="text-black" />
          </div>

          <AiFillStepForward
            onClick={player.next}
            size={30}
            className="text-neutral-400 cursor-pointer hover:text-white transition"
          />

          <button
            onClick={player.toggleRepeat}
            className={`transition ${player.repeatMode !== "off" ? "text-purple-500" : "text-neutral-400 hover:text-white"}`}
          >
            {player.repeatMode === "one" ? <TbRepeatOnce size={22} /> : <TbRepeat size={22} />}
          </button>
      </div>
      
      <div className="flex w-full items-center gap-x-3">
        <p className="text-neutral-400 text-sm w-10 text-right">
          {formatTime(seekValue)}
        </p>
        <Slider
          aria-label="time-indicator"
          size="small"
          value={seekValue}
          min={0}
          step={1}
          max={duration || 100}
          onChange={(_, value) => 
            { 
              sound?.seek(value);
              setSeekValue(value);
            }}
          sx={(t) => ({
            color: '#ffffff',
            height: 4,
            '& .MuiSlider-thumb': {
              width: 8,
              height: 8,
              transition: '0.3s cubic-bezier(.47,1.64,.41,.8)',
              '&::before': {
                boxShadow: '0 2px 12px 0 rgba(0,0,0,0.4)',
              },
              '&:hover, &.Mui-focusVisible': {
                boxShadow: `0px 0px 0px 8px ${'rgb(230 0 255 / 16%)'}`,
                ...t.applyStyles('dark', {
                  boxShadow: `0px 0px 0px 8px ${'rgb(230 104 243 / 16%)'}`,
                }),
              },
              '&.Mui-active': {
                width: 20,
                height: 20,
              },
            },
            '& .MuiSlider-rail': {
              opacity: 0.28,
            },
            ...t.applyStyles('dark', {
              color: '#ffffff',
            }),
          })}
        />
        <p className="text-neutral-400 text-sm w-10">
          {formatTime(duration)}
        </p>
      </div>
    </div>

    <div className="hidden md:flex md:col-span-1 md:col-start-3 items-center justify-end pr-2 gap-x-2">
      <VolumeIcon
          onClick={() => {
            if (volume > 0) {
              setLastVolume(volume);
              player.setVolume(0);
            } else {
              const newVol = lastVolume > 0 ? lastVolume : 1;
              player.setVolume(newVol);
            }
          }}
          size={34}
          className="cursor-pointer"
        />
       <Slider
            aria-label="Volume"
            max={1} 
            value={volume} 
            onChange={(_, value) => player.setVolume(value)} 
            step={0.01}
            sx={(t) => ({
              width: 100,
              color: '#ffff',
              '& .MuiSlider-track': {
                border: 'none',
              },
              '& .MuiSlider-thumb': {
                width: 10,
                height: 10,
                backgroundColor: '#fff',
                '&::before': {
                  boxShadow: '0 4px 8px rgba(0,0,0,0.4)',
                },
                '&:hover, &.Mui-focusVisible, &.Mui-active': {
                  boxShadow: `0px 0px 0px 8px ${'rgb(230 0 255 / 16%)'}`,
                  ...t.applyStyles('dark', {
                    boxShadow: `0px 0px 0px 8px ${'rgb(230 104 243 / 16%)'}`,
                  }),
                },
              },
              ...t.applyStyles('dark', {
                color: '#fff',
              }),
            })}
          />
    </div>
  </div>
);
};

export default PlayerContent;